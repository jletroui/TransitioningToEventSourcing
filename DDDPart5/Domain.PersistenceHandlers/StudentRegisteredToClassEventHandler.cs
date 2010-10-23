using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class StudentRegisteredToClassEventHandler : IHandleEvent<StudentRegisteredToClassEvent>
    {
        private IPersistenceManager persistenceManager;

        public StudentRegisteredToClassEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(StudentRegisteredToClassEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "INSERT INTO [Registration] (aggregateRoot, Id, version, classId, classCredits) VALUES (@Id, @Registration, @Version, @ClassId, @ClassCredits);" +
                "UPDATE [Student] SET registrationSequence = @Registration WHERE Id = @Id;",
                new
                {
                    Id = evt.StudentId,
                    Registration = evt.RegistrationId,
                    Version = DateTime.Now,
                    ClassId = evt.ClassId,
                    ClassCredits = evt.Credits
                });
        }
    }
}
