using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class StudentPassedEventHandler : IHandleEvent<StudentPassedEvent>
    {
        private IPersistenceManager persistenceManager;

        public StudentPassedEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(StudentPassedEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "INSERT passedClasses SELECT @Id, classId FROM Registration WHERE aggregateRoot = @Id AND Id = @Registration;" +
                "DELETE FROM [Registration] WHERE aggregateRoot = @Id AND Id = @Registration;",
                new
                {
                    Id = evt.StudentId,
                    Registration = evt.ClassRegistrationId
                });
        }
    }
}
