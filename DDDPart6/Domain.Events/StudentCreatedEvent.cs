using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Events
{
    [Serializable]
    public class StudentCreatedEvent : IEvent
    {
        public readonly Guid StudentId;

        public StudentCreatedEvent(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
