using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Events
{
    [Serializable]
    public class StudentGraduatedEvent : IEvent
    {
        public readonly Guid StudentId;

        public StudentGraduatedEvent(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
