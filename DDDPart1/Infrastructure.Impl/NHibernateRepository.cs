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
    /// <typeparam name="TEntity"></typeparam>
    public class NHibernateRepository<TEntity> : IRepository<TEntity> where TEntity : IAggregateRoot
    {
        private NHibernatePersistenceManager persistenceManager;

        public NHibernateRepository(NHibernatePersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public TEntity ById(Guid key)
        {
            return persistenceManager.CurrentSession.Get<TEntity>(key);
        }

        public void Add(TEntity toAdd)
        {
            persistenceManager.CurrentSession.SaveOrUpdate(toAdd);
        }

        public void Remove(TEntity toRemove)
        {
            persistenceManager.CurrentSession.Delete(toRemove);
        }

        public IPaginable<TEntity> All()
        {
            return ByCriteria();
        }

        protected virtual IPaginable<TEntity> ByCriteria(params ICriterion[] criteria)
        {
            return new NHibernateCriteriaPaginable<TEntity>(
                persistenceManager,
                criteria,
                new Order[] {Order.Desc("Id")});
        }
    }
}
