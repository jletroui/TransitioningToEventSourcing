using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class StudentCreditsChangedEventHandler : IHandleEvent<StudentCreditsChangedEvent>
    {
        private IPersistenceManager persistenceManager;

        public StudentCreditsChangedEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(StudentCreditsChangedEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "UPDATE [Student] SET credits = @Credits WHERE Id = @Id",
                new
                {
                    Id = evt.StudentId,
                    Credits = evt.Credits
                });
        }
    }
}
