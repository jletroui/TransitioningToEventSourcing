using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Events
{
    [Serializable]
    public class StudentFailedEvent : IEvent
    {
        public readonly Guid StudentId;
        public readonly int ClassRegistrationId;

        public StudentFailedEvent(Guid studentId, int classRegistrationId)
        {
            StudentId = studentId;
            ClassRegistrationId = classRegistrationId;
        }

    }
}
