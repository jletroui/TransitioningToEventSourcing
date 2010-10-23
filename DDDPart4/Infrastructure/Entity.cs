using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using nVentive.Umbrella.Extensions;
using nVentive.Umbrella.Validation;

namespace Infrastructure
{
    /// <summary>
    /// An entity part of an aggregate.
    /// </summary>
    /// <typeparam name="T">The type of aggregate root this entity belongs to.</typeparam>
    public abstract class Entity<T> where T : AggregateRoot
    {
        /// <summary>
        /// Since we will assign ids ourselves to aggregate's entities, this is to allow NHibernate to know when an entity is new.
        /// See http://nhforge.org/blogs/nhibernate/archive/2010/06/30/nhibernate-and-composite-keys.aspx
        /// </summary>
        protected DateTime? version = null;

        /// <summary>
        /// NHibernate constructor.
        /// </summary>
        protected Entity() { }

        public Entity(T aggregateRoot, int id)
        {
            this.aggregateRoot = aggregateRoot.Validation().NotNull("aggregateRoot");
            Id = id;
        }

        private T aggregateRoot;
        public virtual int Id {get; private set;}

        /// <summary>
        /// NHibernate requires that entities with composite ids override Equals.
        /// </summary>
        public override bool Equals(object obj)
        {
            bool resVal = false;

            if (obj != null &&
                obj.GetType() == this.GetType())
            {
                var entity = (Entity<T>)obj;
                resVal = aggregateRoot.Id == entity.aggregateRoot.Id && Id == entity.Id;
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// NHibernate requires that entities with composite ids override Equals. It means we need to override GetHashCode() as well.
        /// </summary>
        public override int GetHashCode()
        {
            return aggregateRoot.Id.GetHashCode() ^ Id.GetHashCode();
        }
    }
}
