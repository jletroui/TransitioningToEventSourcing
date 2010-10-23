using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.ViewModel.Queries
{
    public interface IClassDTOQueries
    {
        IPaginable<ClassDTO> All();
    }
}
