using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Infrastructure.Web;

namespace Presentation.Models
{
    public class StudentSearchModel
    {
        public string Name { get; set; }
        public ISortedPagination<Student> Students { get; set; }
    }
}