using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Infrastructure.Reflection;
using System.Linq.Expressions;

namespace Infrastructure
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private List<IEvent> uncommittedEvents = new List<IEvent>();
        private static Dictionary<Type, Dictionary<Type, Delegate>> applyDelegates = new Dictionary<Type, Dictionary<Type, Delegate>>();

        /// <summary>
        /// This constructor will be used for creating new instances for aggregates.
        /// </summary>
        /// <param name="id"></param>
        public AggregateRoot(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("An aggregate root id can not be empty");
            }
            Id = id;
            Version = 0;
        }

        /// <summary>
        /// This constructor will be used when loading an existing instance of an aggregate.
        /// </summary>
        /// <param name="events"></param>
        public AggregateRoot(IEventInputStream events) : this(events.AggregateId)
        {
            Version = events.Version;

            foreach (var evt in events.Events)
            {
                ExecuteHandler(evt);
            }
        }

        public virtual Guid Id { get; private set; }
        public virtual int Version { get; private set; }
        public virtual IEnumerable<IEvent> UncommitedEvents
        {
            get
            {
                return uncommittedEvents;
            }
        }

        protected virtual void ApplyEvent<T>(T evt) where T : IEvent
        {
            ExecuteHandler(evt);
            uncommittedEvents.Add(evt);
        }

        private void ExecuteHandler<T>(T evt) where T : IEvent
        {
            var handler = applyDelegates[this.GetType()][evt.GetType()] as Action<AggregateRoot, IEvent>;
            if (handler != null) handler(this, evt);
        }

        public static void CreateDelegatesForAggregatesIn(Assembly asm)
        {
            foreach (var agg in from t in asm.GetTypes()
                                where typeof(AggregateRoot).IsAssignableFrom(t)
                                select t)
            {

                Dictionary<Type, Delegate> aggDelegates = new Dictionary<Type, Delegate>();
                applyDelegates.Add(agg, aggDelegates);

                foreach (var applyMethod in from m in agg.GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic)
                                            where m.Name == "Apply" &&
                                                  m.GetParameters().Length == 1 &&
                                                  typeof(IEvent).IsAssignableFrom(m.GetParameters()[0].ParameterType)
                                            select m)
                {
                    Type eventType = applyMethod.GetParameters()[0].ParameterType;

                    // Create an Action<AggregateRoot, IEvent> that does (agg, evt) => ((ConcreteAggregate)agg).Apply((ConcreteEvent)evt)
                    var evtParam = Expression.Parameter(typeof(IEvent), "evt");
                    var aggParam = Expression.Parameter(typeof(AggregateRoot), "agg");
                    aggDelegates.Add(eventType, 
                        Expression.Lambda(
                            Expression.Call(
                                Expression.Convert(
                                    aggParam,
                                    agg),
                                applyMethod,
                                Expression.Convert(
                                    evtParam,
                                    eventType)),
                            aggParam,
                            evtParam).Compile());
                }
            }
        }
    }
}
