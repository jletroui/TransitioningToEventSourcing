using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        IPaginable<Student> ByNameLike(string name);
    }
}
