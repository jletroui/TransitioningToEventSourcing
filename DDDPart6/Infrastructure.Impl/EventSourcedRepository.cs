using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements a repository using NHibernate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventSourcedRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        private IEventStore eventStore;
        private ConstructorInfo constructor;
        private IContext context;

        public EventSourcedRepository(IEventStore eventStore, IContext context)
        {
            this.eventStore = eventStore;
            this.context = context;
            constructor = typeof(T).GetConstructor(new Type[] { typeof(IEventInputStream) });
        }

        public T ById(Guid key)
        {
            var events = eventStore.LoadEventHistory(key);

            var resVal = (T)constructor.Invoke(new Object[] { events });
            AddToContext(resVal);
            return resVal;
        }

        public void Add(T toAdd)
        {
            AddToContext(toAdd);
        }

        private void AddToContext(T toAdd)
        {
            HashSet<IAggregateRoot> aggregates = context[SqlServerPersistenceManager.AGGREGATE_KEY] as HashSet<IAggregateRoot>;

            if (aggregates == null)
            {
                aggregates = new HashSet<IAggregateRoot>();
                context[SqlServerPersistenceManager.AGGREGATE_KEY] = aggregates;
            }

            aggregates.Add(toAdd);
        }
    }
}
