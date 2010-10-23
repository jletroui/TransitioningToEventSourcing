using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Events
{
    public class StudentRegisteredToClassEvent : IEvent
    {
        public readonly Guid StudentId;
        public readonly Guid ClassId;
        public readonly int Credits;

        public StudentRegisteredToClassEvent(Guid studentId, Guid classId, int credits)
        {
            StudentId = studentId;
            ClassId = classId;
            Credits = credits;
        }
    }
}
