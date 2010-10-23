using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Events
{
    public class StudentPassedEvent : IEvent
    {
        public readonly Guid StudentId;
        public readonly int ClassRegistrationId;

        public StudentPassedEvent(Guid studentId, int classRegistrationId)
        {
            StudentId = studentId;
            ClassRegistrationId = classRegistrationId;
        }

    }
}
