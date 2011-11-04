using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Allows to store and retrieve events.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Stores all the uncommited events of an aggregate. If the version do not match the expected one, throws a concurrency exception.
        /// </summary>
        /// <param name="aggregate"></param>
        void PersistUncommitedEvents(IAggregateRoot aggregate);
        /// <summary>
        /// Loads all events for the given aggregate.
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <returns></returns>
        IEventInputStream LoadEventHistory(Guid aggregateId);
    }

    public interface IEventInputStream
    {
        IEnumerable<IEvent> Events { get; }
        int Version { get; }
        Guid AggregateId { get; }
    }

    public class ConcurrencyException : Exception
    {
    }



}
