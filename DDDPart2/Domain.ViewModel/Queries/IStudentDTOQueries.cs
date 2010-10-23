using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.ViewModel.Queries
{
    public interface IStudentDTOQueries
    {
        IPaginable<StudentDTO> ByNameLike(string name);
        StudentDTO ById(Guid id);
        StudentDTO ByIdForRegistration(Guid id);
    }
}
