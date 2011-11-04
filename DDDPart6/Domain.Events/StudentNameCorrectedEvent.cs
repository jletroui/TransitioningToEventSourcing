using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Domain.Events
{
    [Serializable]
    public class StudentNameCorrectedEvent : IEvent
    {
        public readonly Guid StudentId;
        public readonly string FirstName;
        public readonly string LastName;

        public StudentNameCorrectedEvent(Guid studentId, string firstName, string lastName)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
        }

    }
}
