using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Impl;
using Infrastructure;

namespace Domain.ViewModel.Queries
{
    public class SQLStudentDTOQueries : DTOQueries, IStudentDTOQueries
    {
        public SQLStudentDTOQueries(IPersistenceManager pm)
            : base(pm)
        {
        }

        public IPaginable<StudentDTO> ByNameLike(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ByNamedQuery<StudentDTO>("All", null);
            }
            else
            {
                return ByNamedQuery<StudentDTO>("ByNameLike", new { Name = name });
            }
        }

        public StudentDTO ById(Guid id)
        {
            return ByNamedQuery<StudentDTO>("ById", 
                new { Id = id },
                x => x.Registrations).UniqueValue();
        }

        public StudentDTO ByIdForRegistration(Guid id)
        {
            return ByNamedQuery<StudentDTO>("ByIdForRegistration", 
                new { Id = id },
                x => x.NotRegisteredClasses).UniqueValue();
        }
    }
}
