using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// Since we will assign ids ourselves to aggregate's entities, this is to allow NHibernate to know when an entity is new.
        /// See http://nhforge.org/blogs/nhibernate/archive/2010/06/30/nhibernate-and-composite-keys.aspx
        /// </summary>
        protected DateTime? version = null;

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

        public virtual Guid Id {get; private set;}
    }
}
