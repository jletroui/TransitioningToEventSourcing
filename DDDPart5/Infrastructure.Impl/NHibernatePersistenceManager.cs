using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Context;
using System.Data;
using System.Data.SqlClient;
using NHibernate.Cfg;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements a persistence service based on NHibernate.
    /// </summary>
    public class NHibernatePersistenceManager : IPersistenceManager
    {
        private const string SESSION_KEY = "Infrastructure.Impl.NHibernatePersistenceManager.Session";
        private const string TRANSACTION_KEY = "Infrastructure.Impl.NHibernatePersistenceManager.Transaction";
        internal const string AGGREGATE_KEY = "Infrastructure.Impl.NHibernatePersistenceManager.Aggregate";

        private ISessionFactory sessionFactory;
        private IContext context;
        private IEventBus eventBus;

        public NHibernatePersistenceManager(ISessionFactory sessionFactory, IContext context, IEventBus eventBus)
        {
            this.sessionFactory = sessionFactory;
            this.context = context;
            this.eventBus = eventBus;
        }

        public ISession CurrentSession
        {
            get
            {
                return context[SESSION_KEY] as ISession;
            }
        }

        public void Open()
        {
            if (IsOpened())
            {
                throw new InvalidOperationException("The persistence manager is already opened");
            }

            context[SESSION_KEY] = sessionFactory.OpenSession();
        }

        public bool IsOpened()
        {
            return CurrentSession != null &&
                CurrentSession.IsOpen;
        }

        public void Close()
        {
            var session = EnsureOpened();
            context[SESSION_KEY] = null;
            session.Close();
        }

        public void Commit()
        {
            var aggregates = context[AGGREGATE_KEY] as HashSet<IAggregateRoot>;

            if (aggregates != null && aggregates.Count > 0)
            {
                var session = EnsureOpened();

                using (var tx = session.Connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    context[TRANSACTION_KEY] = tx;

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
            var session = EnsureOpened();

            var resVal = session.Connection.CreateCommand();
            resVal.Transaction = context[TRANSACTION_KEY] as IDbTransaction;
            return resVal;
        }

        private ISession EnsureOpened()
        {
            if (!IsOpened())
            {
                throw new InvalidOperationException("The persistence manager is not opened");
            }

            return CurrentSession;
        }

        public void Dispose()
        {
            if (sessionFactory != null)
            {
                if (IsOpened())
                {
                    Close();
                }
                sessionFactory.Dispose();
            }
        }

    }
}
