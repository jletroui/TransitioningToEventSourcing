using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Commands
{
    public class CorrectStudentNameCommand : ICommand
    {
        public readonly Guid StudentId;
        public readonly string FirstName;
        public readonly string LastName;

        public CorrectStudentNameCommand(Guid studentId, string firstName, string lastName)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
