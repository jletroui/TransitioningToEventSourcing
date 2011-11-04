using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Commands
{
    public abstract class RegisterStudentResultCommandBase : ICommand
    {
        public readonly Guid StudentId;
        public readonly int ClassRegistrationId;

        public RegisterStudentResultCommandBase(Guid studentId, int classRegistrationId)
        {
            StudentId = studentId;
            ClassRegistrationId = classRegistrationId;
        }
    }
}
