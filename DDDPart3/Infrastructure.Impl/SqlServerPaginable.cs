using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Validation;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using nVentive.Umbrella.Extensions;
using nVentive.Umbrella.Validation;
using System.Data;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements <see cref="IPaginable{T}"/> for raw SQL queries for SQL2005+.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SqlServerPaginable<T> : IPaginable<T> where T : new()
    {
        private string countQuery;
        private string selectQuery;
        private string rootQuery = null;
        private string defaultSort;
        private IDictionary<string, object> parameters;
        private IPersistenceManager persistenceManager;
        private Expression<Func<T, object>>[] resultSetMappings;

        private static readonly Regex ORDER_BY_REGEX = new Regex(@"\sorder\sby.*", RegexOptions.IgnoreCase);
        private static readonly Regex SELECT_REGEX = new Regex("^select", RegexOptions.IgnoreCase);

        public SqlServerPaginable(IPersistenceManager persistenceManager,
            string countQuery,
            string selectQuery,
            IDictionary<string, object> parameters,
            string defaultSort,
            Expression<Func<T, object>>[] resultSetMappings)
        {
            this.persistenceManager = persistenceManager;
            this.countQuery = countQuery;
            this.selectQuery = selectQuery;
            this.parameters = parameters;
            this.defaultSort = defaultSort.Validation().NotEmpty("defaultSort");
            this.resultSetMappings = resultSetMappings;
        }

        public int Count()
        {
            return (int)GetBaseQuery(countQuery).ExecuteScalar();
        }

        public T UniqueValue()
        {
            using (var reader = GetBaseQuery(selectQuery).ExecuteReader())
            {
                return new DTOMapper().Map<T>(
                    reader,
                    resultSetMappings);
            }
        }

        public IEnumerable<T> ToEnumerable()
        {
            using (var reader = GetBaseQuery(selectQuery).ExecuteReader())
            {
                return new DTOMapper().MapList<T>(reader);
            }
        }

        public IEnumerable<T> ToEnumerable(int skip, int take)
        {
            return ToEnumerable(skip, take, null, null);
        }

        public IEnumerable<T> ToEnumerable(int skip, int take, string sortColumn, SortDirection? sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortColumn))
            {
                sortColumn = defaultSort;
            }

            var sorts = string.Format("{0} {1}", 
                sortColumn, 
                sortDirection.GetValueOrDefault() == SortDirection.Asc ? "ASC" : "DESC");

            return DoListPage(GetBaseQuery(GetSelectStatementWithoutOrderClause()), skip, take, sorts);
        }

        private string GetSelectStatementWithoutOrderClause()
        {
            if (rootQuery == null)
            {
                string initialQuery = selectQuery;
                MatchCollection matches = ORDER_BY_REGEX.Matches(initialQuery);
                if (matches.Count > 1)
                {
                    rootQuery = null;
                    throw new OrderByException(initialQuery);
                }
                else if (matches.Count == 1)
                {
                    rootQuery = initialQuery.Substring(0, matches[0].Index);
                }
                else
                {
                    rootQuery = initialQuery;
                }
            }

            return rootQuery;
        }

        private IDbCommand GetBaseQuery(string query)
        {
            var cmd = persistenceManager.CreateCommand();
            cmd.CommandText = query;

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = kvp.Key.StartsWith("@") ? kvp.Key : "@" + kvp.Key;
                    param.Value = kvp.Value;
                    cmd.Parameters.Add(param);
                }
            }

            return cmd;
        }

        private IEnumerable<T> DoListPage(IDbCommand query, int offset, int limit, string sorts)
        {
            if (offset > 0 && limit > 0)
            {
                AddPaginationToQuery(query, limit, offset, sorts);
            }
            else if (offset > 0)
            {
                AddOffsetToQuery(query, offset, sorts);
            }
            else if (limit > 0)
            {
                AddTopToQuery(query, limit, sorts);
            }
            else
            {
                AddSortsToQuery(query, sorts);
            }

            using (var reader = query.ExecuteReader())
            {
                return new DTOMapper().MapList<T>(reader);
            }
        }

        private void AddSortsToQuery(IDbCommand query, string sorts)
        {
            query.CommandText += string.IsNullOrEmpty(sorts) ? string.Empty : " ORDER BY " + sorts;
        }

        private void AddTopToQuery(IDbCommand query, int top, string sorts)
        {
            query.CommandText = SELECT_REGEX.Replace(query.CommandText,
                string.Format("SELECT TOP {0}", top));
            AddSortsToQuery(query, sorts);
        }

        private void AddOffsetToQuery(IDbCommand query, int offset, string sorts)
        {
            query.CommandText = string.Format(
                "select * from ({0}) results  where RowNumber > {1}",
                SELECT_REGEX.Replace(
                    query.CommandText,
                    string.Format("select ROW_NUMBER() over (order by {0}) as RowNumber, ", sorts)),
                offset);
        }
        private void AddPaginationToQuery(IDbCommand query, int top, int offset, string sorts)
        {
            query.CommandText = string.Format(
                "select top {2} * from ({0}) results  where RowNumber > {1}",
                SELECT_REGEX.Replace(
                    query.CommandText,
                    string.Format("select ROW_NUMBER() over (order by {0}) as RowNumber, ", sorts)),
                offset,
                top);
        }

    }

    [Serializable]
    public class OrderByException : InvalidOperationException
    {
        private const string INTERNAL_MESSAGE = "Unable to deal with more than one ORDER BY (query was {0})";

        public OrderByException(string initialQuery) : base(string.Format(INTERNAL_MESSAGE, initialQuery)) { }
    }

}
