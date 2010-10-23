using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Context;
using System.Data;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Implements a persistence service based on NHibernate.
    /// </summary>
    public class NHibernatePersistenceManager : IPersistenceManager
    {
        private ISessionFactory sessionFactory;

        public NHibernatePersistenceManager(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public ISession CurrentSession
        {
            get
            {
                return sessionFactory.GetCurrentSession();
            }
        }

        public void Open()
        {
            if (IsOpened())
            {
                throw new InvalidOperationException("The persistence manager is already opened");
            }

            ISession session = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        public bool IsOpened()
        {
            return CurrentSessionContext.HasBind(sessionFactory) && 
                sessionFactory.GetCurrentSession().IsOpen;
        }

        public void Close()
        {
            var session = EnsureOpened();

            CurrentSessionContext.Unbind(sessionFactory);
            session.Close();
        }

        public void Commit()
        {
            var session = EnsureOpened();

            if (!session.Transaction.IsActive)
            {
                session.BeginTransaction();
            }
            session.Transaction.Commit();
        }

        public void Flush()
        {
            var session = EnsureOpened();

            if (session.IsDirty())
            {
                if (!session.Transaction.IsActive)
                {
                    session.BeginTransaction();
                }

                session.Flush();
            }
        }

        public IDbCommand CreateCommand()
        {
            var session = EnsureOpened();
            Flush();

            var resVal = session.Connection.CreateCommand();
            session.Transaction.Enlist(resVal);

            return resVal;
        }

        private ISession EnsureOpened()
        {
            if (!IsOpened())
            {
                throw new InvalidOperationException("The persistence manager is not opened");
            }

            return sessionFactory.GetCurrentSession();
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
