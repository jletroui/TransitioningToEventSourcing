using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Defines what's constitute an aggregate root.
    /// </summary>
    public interface IAggregateRoot
    {
        Guid Id { get; }
        IEnumerable<IEvent> UncommitedEvents { get; }
        int Version { get; }
    }
}
