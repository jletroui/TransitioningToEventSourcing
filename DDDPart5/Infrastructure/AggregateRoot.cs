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
        /// <summary>
        /// Since we will assign ids ourselves to aggregate's entities, this is to allow NHibernate to know when an entity is new.
        /// See http://nhforge.org/blogs/nhibernate/archive/2010/06/30/nhibernate-and-composite-keys.aspx
        /// </summary>
        protected DateTime? version = null;
        private List<IEvent> uncommittedEvents = new List<IEvent>();
        private static Dictionary<Type, Dictionary<Type, Delegate>> applyDelegates = new Dictionary<Type, Dictionary<Type, Delegate>>();

        /// <summary>
        /// For NHibernate constructors.
        /// </summary>
        protected AggregateRoot()
        {
        }

        public AggregateRoot(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("An aggregate root id can not be empty");
            }
            Id = id;
        }

        public virtual Guid Id { get; private set; }
        public virtual IEnumerable<IEvent> UncommitedEvents
        {
            get
            {
                return uncommittedEvents;
            }
        }

        protected virtual void ApplyEvent<T>(T evt) where T : IEvent
        {
            var handler = applyDelegates[this.GetType()][typeof(T)] as Action<AggregateRoot, T>;
            handler(this, evt);

            uncommittedEvents.Add(evt);
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

                    // Create an Action<AggregateRoot, SomeEvent> that does (agg, evt) => ((ConcreteAggregate)agg).Apply(evt)
                    var evtParam = Expression.Parameter(eventType, "evt");
                    var aggParam = Expression.Parameter(typeof(AggregateRoot), "agg");
                    aggDelegates.Add(eventType, 
                        Expression.Lambda(
                            Expression.Call(
                                Expression.Convert(
                                    aggParam,
                                    agg),
                                applyMethod,
                                evtParam),
                            aggParam,
                            evtParam).Compile());
                }
            }
        }
    }
}
