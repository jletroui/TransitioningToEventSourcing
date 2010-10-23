using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class StudentFailedEventHandler : IHandleEvent<StudentFailedEvent>
    {
        private IPersistenceManager persistenceManager;

        public StudentFailedEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(StudentFailedEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "INSERT failedClasses SELECT @Id, classId FROM Registration WHERE aggregateRoot = @Id AND Id = @Registration;" +
                "DELETE FROM [Registration] WHERE aggregateRoot = @Id AND Id = @Registration;",
                new
                {
                    Id = evt.StudentId,
                    Registration = evt.ClassRegistrationId
                });
        }
    }
}
