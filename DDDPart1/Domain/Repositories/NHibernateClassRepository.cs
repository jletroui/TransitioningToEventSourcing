using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Impl;
using Infrastructure;

namespace Domain.Repositories
{
    public class NHibernateClassRepository : NHibernateRepository<Class>, IClassRepository
    {
        public NHibernateClassRepository(NHibernatePersistenceManager pm)
            : base(pm)
        {
        }
    }
}
