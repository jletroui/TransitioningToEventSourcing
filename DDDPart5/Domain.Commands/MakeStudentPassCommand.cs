using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Commands
{
    public class MakeStudentPassCommand : RegisterStudentResultCommandBase
    {
        public MakeStudentPassCommand(Guid studentId, int classRegistrationId)
            : base(studentId, classRegistrationId)
        {
        }
    }
}
