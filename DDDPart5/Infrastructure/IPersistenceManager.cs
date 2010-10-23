using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure
{
    /// <summary>
    /// Abstracts the persistence mechanism that is used for persisting aggregates.
    /// </summary>
    public interface IPersistenceManager : IDisposable
    {
        /// <summary>
        /// Opens a new transaction for the current thread or context.
        /// </summary>
        void Open();
        /// <summary>
        /// Whether the persistence manager currently has an opened session.
        /// </summary>
        /// <returns>true if an opened session is present.</returns>
        bool IsOpened();
        /// <summary>
        /// Closes the current transaction.  If the transaction was not committed,
        /// roll it back first.
        /// </summary>
        void Close();
        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        void Commit();
        /// <summary>
        /// Creates a command in the current transaction.
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand();
    }
}
