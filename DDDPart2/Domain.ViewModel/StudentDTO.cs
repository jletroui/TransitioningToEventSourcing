using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class StudentDTO
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }
        public bool HasGraduated { get; set; }
        public Guid ClassToRegister { get; set; }
        public List<RegistrationDTO> Registrations { get; set; }
        public List<ClassDTO> NotRegisteredClasses { get; set; }
    }
}
