using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements <see cref="ICommandBus"/> using an IoC container.
    /// </summary>
    public class ContainerCommandBus : ICommandBus
    {
        private IContainer container;
        private IPersistenceManager persistenceManager;

        public ContainerCommandBus(IContainer container, IPersistenceManager persistenceManager)
        {
            this.container = container;
            this.persistenceManager = persistenceManager;
        }

        public void Send<T>(T cmd) where T : ICommand
        {
            var handler = container.Build<IHandleCommand<T>>();
            var handled = false;

            while (handled == false)
            {
                try
                {
                    handler.Handle(cmd);
                    // Trigger persistence and concurrency checking.
                    persistenceManager.Commit();
                    handled = true;
                }
                catch (ConcurrencyException)
                {
                    // Hit a concurrency exception, must retry the command.
                }
            }
        }
    }
}
