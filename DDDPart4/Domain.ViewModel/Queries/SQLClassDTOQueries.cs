using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Impl;
using Infrastructure;

namespace Domain.ViewModel.Queries
{
    public class SQLClassDTOQueries : DTOQueries, IClassDTOQueries
    {
        public SQLClassDTOQueries(IPersistenceManager pm)
            : base(pm)
        {
        }

        public IPaginable<ClassDTO> All()
        {
            return ByNamedQuery<ClassDTO>("All", null);
        }
    }
}
