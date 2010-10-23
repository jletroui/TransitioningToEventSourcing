using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using nVentive.Umbrella.Extensions;
using nVentive.Umbrella.Validation;
using Domain.Events;

namespace Domain
{
    public class Class : AggregateRoot
    {
        private string name;
        internal int credits;

        /// <summary>
        /// NHibernate constructor.
        /// </summary>
        protected Class() { }

        public Class(Guid id, string name, int credits)
            : base(id)
        {
            // Business rules
            if (string.IsNullOrWhiteSpace(name) || name.Length > 255)
            {
                throw new ArgumentException("name must be a text that have between 1 and 255 characters");
            }
            if (credits < 3 || credits > 6)
            {
                throw new ArgumentException("credits must be between 3 and 6");
            }

            // State changes
            Apply(new ClassCreatedEvent(id, name, credits));
        }

        public void Apply(ClassCreatedEvent evt)
        {
            name = evt.Name;
            credits = evt.Credits;
        }
    }
}
