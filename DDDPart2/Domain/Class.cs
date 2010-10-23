using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using nVentive.Umbrella.Extensions;
using nVentive.Umbrella.Validation;

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

        public Class(string name, int credits)
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
            this.name = name;
            this.credits = credits;
        }

    }
}
