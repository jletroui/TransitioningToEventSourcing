using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// A bus allowing events to be published.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publish an event (notify all handler interested by this type of event).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="evt"></param>
        void Publish<T>(T evt) where T : IEvent;
    }
}
