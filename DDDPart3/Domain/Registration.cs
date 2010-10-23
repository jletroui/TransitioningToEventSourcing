using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain
{
    public class Registration : Entity<Student>
    {
        internal Class _class;

        /// <summary>
        /// NHibernate constructor.
        /// </summary>
        protected Registration() { }

        public Registration(Student student, int id, Class @class)
            : base(student, id)
        {
            _class = @class;
        }
    }
}
