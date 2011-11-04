using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;

namespace Domain.PersistenceHandlers
{
    public class ClassCreatedEventHandler : IHandleEvent<ClassCreatedEvent>
    {
        private IPersistenceManager persistenceManager;

        public ClassCreatedEventHandler(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void Handle(ClassCreatedEvent evt)
        {
            persistenceManager.ExecuteNonQuery(
                "INSERT INTO [Class] (Id, version, name, credits) VALUES (@Id, @Version, @Name, @Credits)",
                new
                {
                    Id = evt.ClassId,
                    Version = DateTime.Now,
                    Name = evt.Name,
                    Credits = evt.Credits
                });
        }
    }
}
