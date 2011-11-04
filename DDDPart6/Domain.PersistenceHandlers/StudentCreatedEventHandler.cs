using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class StudentCreatedEventHandler : IHandleEvent<StudentCreatedEvent>
    {
        private IPersistenceManager persistenceManager;

        public StudentCreatedEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(StudentCreatedEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "INSERT INTO [Student] (Id, version) VALUES (@Id, @Version)",
                new
                {
                    Id = evt.StudentId,
                    Version = DateTime.Now,
                });
        }
    }
}
