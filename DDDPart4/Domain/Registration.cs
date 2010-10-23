using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain
{
    public class Registration : Entity<Student>
    {
        private Guid classId;
        private int classCredits;

        /// <summary>
        /// NHibernate constructor.
        /// </summary>
        protected Registration() { }

        public Registration(Student student, int id, Guid classId, int classCredits)
            : base(student, id)
        {
            this.classCredits = classCredits;
            this.classId  = classId;
        }

        internal int ClassCredits
        {
            get
            {
                return classCredits;
            }
        }

        internal Guid ClassId
        {
            get
            {
                return classId;
            }
        }
    }
}
