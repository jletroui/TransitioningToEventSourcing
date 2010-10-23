using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements a repository using NHibernate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NHibernateRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        private NHibernatePersistenceManager persistenceManager;
        private IContext context;

        public NHibernateRepository(NHibernatePersistenceManager persistenceManager, IContext context)
        {
            this.persistenceManager = persistenceManager;
            this.context = context;
        }

        public T ById(Guid key)
        {
            var resVal =  persistenceManager.CurrentSession.Get<T>(key);
            AddToContext(resVal);
            return resVal;
        }

        public void Add(T toAdd)
        {
            AddToContext(toAdd);
        }

        private void AddToContext(T toAdd)
        {
            HashSet<IAggregateRoot> aggregates = context[NHibernatePersistenceManager.AGGREGATE_KEY] as HashSet<IAggregateRoot>;

            if (aggregates == null)
            {
                aggregates = new HashSet<IAggregateRoot>();
                context[NHibernatePersistenceManager.AGGREGATE_KEY] = aggregates;
            }

            aggregates.Add(toAdd);
        }
    }
}
