using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Represents a class that will react to a given event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHandleEvent<T> where T : IEvent
    {
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="evt"></param>
        void Handle(T evt);
    }
}
