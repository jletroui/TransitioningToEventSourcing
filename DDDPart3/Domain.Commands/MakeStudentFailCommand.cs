using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Commands
{
    public class MakeStudentFailCommand : RegisterStudentResultCommandBase
    {
        public MakeStudentFailCommand(Guid studentId, int classRegistrationId)
            : base(studentId, classRegistrationId)
        {
        }
    }
}
