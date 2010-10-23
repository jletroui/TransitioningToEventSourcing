using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Commands
{
    public class RegisterStudentToClassCommand : ICommand
    {
        public readonly Guid StudentId;
        public readonly Guid ClassId;

        public RegisterStudentToClassCommand(Guid studentId, Guid classId)
        {
            StudentId = studentId;
            ClassId = classId;
        }
    }
}
