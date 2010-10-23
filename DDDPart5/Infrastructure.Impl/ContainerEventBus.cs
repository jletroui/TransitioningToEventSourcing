using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Impl
{
    public class ContainerEventBus : IEventBus
    {
        private IContainer container;

        public ContainerEventBus(IContainer container)
        {
            this.container = container;
        }

        public void Publish<T>(T evt) where T : IEvent
        {
            var handlerType = typeof(IHandleEvent<>).MakeGenericType(evt.GetType());

            foreach (var handler in container.BuildAll<object>(handlerType))
            {
                handlerType.GetMethod("Handle").Invoke(handler, new object[] { evt });
            }
        }

    }
}
