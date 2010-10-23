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

        public NHibernateRepository(NHibernatePersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public T ById(Guid key)
        {
            return persistenceManager.CurrentSession.Get<T>(key);
        }

        public void Add(T toAdd)
        {
            persistenceManager.CurrentSession.SaveOrUpdate(toAdd);
        }

        public void Remove(T toRemove)
        {
            persistenceManager.CurrentSession.Delete(toRemove);
        }
    }
}
