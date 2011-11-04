using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Events
{
    [Serializable]
    public class StudentCreditsChangedEvent : IEvent
    {
        public readonly Guid StudentId;
        public readonly int Credits;

        public StudentCreditsChangedEvent(Guid studentId, int credits)
        {
            StudentId = studentId;
            Credits = credits;
        }
    }
}
