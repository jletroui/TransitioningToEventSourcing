using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace Presentation.Models
{
    public class RegisterToClassModel
    {
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public string StudentName { get; set; }
        public IEnumerable<Class> Classes { get; set; }
    }
}