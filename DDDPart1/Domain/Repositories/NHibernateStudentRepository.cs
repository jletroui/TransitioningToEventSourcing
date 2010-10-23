using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Impl;
using NHibernate.Criterion;

namespace Domain.Repositories
{
    public class NHibernateStudentRepository : NHibernateRepository<Student>, IStudentRepository
    {
        public NHibernateStudentRepository(NHibernatePersistenceManager pm)
            : base(pm)
        {
        }

        #region IStudentRepository Members

        public Infrastructure.IPaginable<Student> ByNameLike(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return All();
            }
            else
            {
                return ByCriteria(
                    Restrictions.Or(
                        Restrictions.Like("FirstName", name),
                        Restrictions.Like("LastName", name)));
            }
        }

        #endregion

    }
}
