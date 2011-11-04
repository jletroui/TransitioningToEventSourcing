using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class StudentNameCorrectedEventHandler : IHandleEvent<StudentNameCorrectedEvent>
    {
        private IPersistenceManager persistenceManager;

        public StudentNameCorrectedEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(StudentNameCorrectedEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "UPDATE [Student] SET firstName = @FirstName, lastName = @LastName WHERE Id = @Id",
                new
                {
                    Id = evt.StudentId,
                    FirstName = evt.FirstName,
                    LastName = evt.LastName
                });

        }
    }
}
