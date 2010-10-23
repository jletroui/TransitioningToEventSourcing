using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class StudentGraduatedEventHandler : IHandleEvent<StudentGraduatedEvent>
    {
        private IPersistenceManager persistenceManager;

        public StudentGraduatedEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(StudentGraduatedEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "UPDATE [Student] SET hasGraduated = 1 WHERE Id = @Id",
                new
                {
                    Id = evt.StudentId
                });
        }
    }
}
