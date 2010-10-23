using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Xml;

using Infrastructure.Reflection;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Offers basic services for easily create DTO queries.
    /// </summary>
    public abstract class DTOQueries
    {
        #region QueryCache

        private static Dictionary<Type, Dictionary<string, QueryInfo>> queries = new Dictionary<Type, Dictionary<string, QueryInfo>>();

        private static void EnsureType(Type type)
        {
            if (!queries.ContainsKey(type))
            {
                lock (queries)
                {
                    if (!queries.ContainsKey(type))
                    {
                        queries.Add(type, new Dictionary<string, QueryInfo>());

                        try
                        {
                            ExtractQueries(type, type.FullName + ".xml");
                        }
                        catch { }
                    }
                }
            }
        }
        private static void ExtractQueries(Type type, string queryFile)
        {
            using (var stream = type.Assembly.GetManifestResourceStream(queryFile))
            {
                if (stream != null)
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        if (MoveToNextElement(reader))
                        {
                            // For each query element
                            while (MoveToNextElement(reader))
                            {
                                // Get the name attribute
                                reader.MoveToFirstAttribute();
                                string name = reader.Value;

                                // Get the default sort attribute
                                reader.MoveToNextAttribute();
                                string defaultSort = reader.Value;

                                // Move to the count element
                                MoveToNextElement(reader);
                                reader.Read(); // Move to text or CDATA node
                                string countQuery = reader.Value;

                                // Move to the select element
                                MoveToNextElement(reader);
                                reader.Read(); // Move to text or CDATA node
                                string selectQuery = reader.Value;

                                queries[type].Add(name, new QueryInfo(countQuery, selectQuery, defaultSort));
                            }
                        }
                    }
                }
            }
        }

        private static bool MoveToNextElement(XmlTextReader reader)
        {
            bool resVal = true;
            while ((resVal = reader.Read()) && reader.NodeType != XmlNodeType.Element) ;
            return resVal;
        }

        public static IDictionary<string, QueryInfo> Queries(DTOQueries dtoQueries)
        {
            EnsureType(dtoQueries.GetType());
            return queries[dtoQueries.GetType()];
        }
        #endregion

        private IPersistenceManager pm;

        public DTOQueries(IPersistenceManager pm)
        {
            this.pm = pm;
        }

        /// <summary>
        /// Create a query for the given name.
        /// Queries must be located in an xml file with the same name as the class.
        /// Ex: for Domain.ViewModel.Queries.SQLStudentDTOQueries, there should be an embedded resource:
        /// Domain.ViewModel.Queries.SQLStudentDTOQueries.xml.
        /// </summary>
        protected IPaginable<T> ByNamedQuery<T>(
            string name,
            object parameters,
            params Expression<Func<T, object>>[] collectionMappings) where T : new()
        {
            if (!Queries(this).ContainsKey(name))
            {
                throw new ArgumentException(string.Format(
                    "Query with the name '{0}' can not be found. Make sure the query xml file is present and is an embedded resource.",
                    name));
            }

            var query = Queries(this)[name];

            return new SqlServerPaginable<T>(
                pm,
                query.CountQuery,
                query.SelectQuery,
                parameters.ToDictionary(),
                query.DefaultSort,
                collectionMappings);
        }
    }

    public class QueryInfo
    {
        public readonly string CountQuery;
        public readonly string SelectQuery;
        public readonly string DefaultSort;

        public QueryInfo(string countQuery, string selectQuery, string defaultSort)
        {
            CountQuery = countQuery;
            SelectQuery = selectQuery;
            DefaultSort = defaultSort;
        }
    }
}
