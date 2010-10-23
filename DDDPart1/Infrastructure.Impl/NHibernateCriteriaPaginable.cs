using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NHibernate.Criterion;
using NHibernate;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements a paginable based on NHibernate's criteria API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class NHibernateCriteriaPaginable<T> : IPaginable<T>
    {
        private ICriterion[] criteria;
        private Order[] order;
        private NHibernatePersistenceManager persistenceManager;

        public NHibernateCriteriaPaginable(NHibernatePersistenceManager pm,
            ICriterion[] criteria,
            Order[] order)
        {
            this.persistenceManager = pm;
            this.criteria = criteria;
            if (order == null)
            {
                this.order = new Order[0];
            }
            else
            {
                this.order = order;
            }
        }

        public int Count()
        {
            persistenceManager.Flush();
            ICriteria baseCriteria = GetBaseCriteria(null);
            baseCriteria.SetProjection(Projections.RowCount());
            return (int)baseCriteria.List()[0];
        }

        public T UniqueValue()
        {
            persistenceManager.Flush();
            return GetBaseCriteria(order).UniqueResult<T>();
        }

        public IEnumerable<T> ToEnumerable()
        {
            persistenceManager.Flush();
            return GetBaseCriteria(order).List<T>();
        }

        public IEnumerable<T> ToEnumerable(int skip, int take)
        {
            return ToEnumerable(skip, take, null, null);
        }

        public IEnumerable<T> ToEnumerable(int skip, int take, string sortColumn, SortDirection? sortDirection)
        {
            Order[] order = this.order;

            if (sortColumn != null && sortDirection != null)
            {
                order = new Order[] { TranslateSortOrder(sortColumn, sortDirection.Value) };
            }

            return DoListPage(skip, take, order);
        }

        private static Order TranslateSortOrder(string propertyName, SortDirection order)
        {
            if (String.IsNullOrEmpty(propertyName))
                return null;

            if (order == SortDirection.Asc)
                return Order.Asc(propertyName);
            else
                return Order.Desc(propertyName);
        }

        private ICriteria GetBaseCriteria(Order[] order)
        {
            ICriteria baseCriteria = persistenceManager.CurrentSession.CreateCriteria(typeof(T));

            foreach (ICriterion c in criteria)
            {
                baseCriteria.Add(c);
            }

            if (order != null)
            {
                foreach (Order o in order)
                {
                    if (o != null)
                    {
                        baseCriteria.AddOrder(o);
                    }
                }
            }

            return baseCriteria;
        }

        private IEnumerable<T> DoListPage(int skip, int take, params Order[] order)
        {
            persistenceManager.Flush();

            ICriteria baseCriteria = GetBaseCriteria(order);

            if (skip > 0)
            {
                baseCriteria.SetFirstResult(skip);
            }

            if (take > 0)
            {
                baseCriteria.SetMaxResults(take);
            }

            return baseCriteria.List<T>();
        }
    }
}
