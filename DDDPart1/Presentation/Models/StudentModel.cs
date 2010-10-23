using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace Presentation.Models
{
    public class StudentModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength=1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }
    }
}