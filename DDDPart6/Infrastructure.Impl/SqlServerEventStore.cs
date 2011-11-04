using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Data;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastructure.Impl
{
    public class SqlServerEventStore : IEventStore
    {
        private IPersistenceManager persistenceManager;
        private BinaryFormatter serializer = new BinaryFormatter();

        public SqlServerEventStore(IPersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
        }

        public void PersistUncommitedEvents(IAggregateRoot aggregate)
        {
            persistenceManager.ExecuteNonQuery(
                "INSERT INTO [Events] (Id, aggregate_id, version, data) VALUES (@Id, @AggregateId, @Version, @Data)",
                new
                {
                    Id = Guid.NewGuid(),
                    Version = aggregate.Version + 1,
                    AggregateId = aggregate.Id,
                    Data = Serialize(aggregate.UncommitedEvents)
                });

            if (aggregate.Version == 0)
            {
                persistenceManager.ExecuteNonQuery(
                    "INSERT INTO [Aggregates] (aggregate_id, version) VALUES (@AggregateId, 1)",
                    new
                    {
                        AggregateId = aggregate.Id
                    });
            }
            else
            {
                var rowCount = persistenceManager.ExecuteNonQuery(
                    "UPDATE [Aggregates] SET Version = @Version WHERE aggregate_id = @AggregateId AND Version = @Expected",
                    new
                    {
                        AggregateId = aggregate.Id,
                        Version = aggregate.Version + 1,
                        Expected = aggregate.Version
                    });

                // The version evolved since we performed the command, so we have to retry the command.
                if (rowCount != 1) throw new ConcurrencyException();
            }
        }

        public IEventInputStream LoadEventHistory(Guid aggregateId)
        {
            using (var reader = persistenceManager.ExecuteQuery("SELECT [Version], [Data] FROM [Events] WHERE aggregate_id = @AggregateId ORDER BY [Version] ASC", new { AggregateId = aggregateId }))
            {
                var events = new List<IEvent>();
                var version = 0;

                while (reader.Read())
                {
                    version = reader.GetInt32(0);
                    var data = ((SqlDataReader)reader).GetSqlBinary(1).Value;
                    events.AddRange(Deserialize(data));
                }

                return new SimpleEventInputStream(events, version, aggregateId);
            }
        }

        private class SimpleEventInputStream : IEventInputStream
        {
            public SimpleEventInputStream(IEnumerable<IEvent> events, int version, Guid aggregateId)
            {
                Events = events;
                Version = version;
                AggregateId = aggregateId;
            }

            public IEnumerable<IEvent> Events { get; private set; }
            public int Version { get; private set; }
            public Guid AggregateId { get; private set; }
        }

        private byte[] Serialize(IEnumerable<IEvent> events)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, events.ToArray());
                return stream.GetBuffer();
            }
        }

        private IEnumerable<IEvent> Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return (IEvent[])serializer.Deserialize(stream);
            }
        }
    }
}
