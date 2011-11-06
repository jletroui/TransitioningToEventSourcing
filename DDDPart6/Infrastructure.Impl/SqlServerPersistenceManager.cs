using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements a persistence service based on direct ADO connections.
    /// </summary>
    public class SqlServerPersistenceManager : IPersistenceManager
    {
        private const string CONNECTION_KEY = "Infrastructure.Impl.ADOPersistenceManager.Connection";
        private const string TRANSACTION_KEY = "Infrastructure.Impl.ADOPersistenceManager.Transaction";
        internal const string AGGREGATE_KEY = "Infrastructure.Impl.ADOPersistenceManager.Aggregate";

        private IContext context;
        private IEventBus eventBus;
        private IEventStore eventStore = null;
        private IContainer container;
        private string connectionString;

        public SqlServerPersistenceManager(string connectionString, IContext context, IEventBus eventBus, IContainer container)
        {
            this.connectionString = connectionString;
            this.context = context;
            this.eventBus = eventBus;
            this.container = container;
        }

        private IEventStore lazyEventStore
        {
            get
            {
                if (eventStore == null) eventStore = container.Build<IEventStore>();
                return eventStore;
            }
        }

        public IDbConnection CurrentConnection
        {
            get
            {
                return context[CONNECTION_KEY] as IDbConnection;
            }
        }


        public void Open()
        {
            if (IsOpened())
            {
                throw new InvalidOperationException("The persistence manager is already opened");
            }

            var connection = new SqlConnection(connectionString);
            connection.Open();
            context[CONNECTION_KEY] = connection;
        }

        public bool IsOpened()
        {
            return CurrentConnection != null &&
                CurrentConnection.State == ConnectionState.Open;
        }

        public void Close()
        {
            var session = EnsureOpened();
            context[CONNECTION_KEY] = null;
            session.Close();
        }

        public void Commit()
        {
            var aggregates = context[AGGREGATE_KEY] as HashSet<IAggregateRoot>;

            if (aggregates != null && aggregates.Count > 0)
            {
                var connection = EnsureOpened();

                using (var tx = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    context[TRANSACTION_KEY] = tx;

                    try
                    {
                        foreach (var ar in aggregates)
                        {
                            lazyEventStore.PersistUncommitedEvents(ar);
                        }
                    }
                    catch (ConcurrencyException)
                    {
                        tx.Rollback();
                        context[TRANSACTION_KEY] = null;
                        throw;
                    }

                    // At this stage, no concurrency issues, so pass on to the event handlers
                    foreach (var ar in aggregates)
                    {
                        foreach (var evt in ar.UncommitedEvents)
                        {
                            eventBus.Publish(evt);
                        }
                    }

                    context[TRANSACTION_KEY] = null;
                    tx.Commit();

                    context[AGGREGATE_KEY] = null;
                }
            }
        }

        public IDbCommand CreateCommand()
        {
            var connection = EnsureOpened();

            var resVal = connection.CreateCommand();
            resVal.Transaction = context[TRANSACTION_KEY] as IDbTransaction;
            return resVal;
        }

        private IDbConnection EnsureOpened()
        {
            if (!IsOpened())
            {
                throw new InvalidOperationException("The persistence manager is not opened");
            }

            return CurrentConnection;
        }

        public void Dispose()
        {
            if (IsOpened())
            {
                Close();
            }
        }

    }
}
