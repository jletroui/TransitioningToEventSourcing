using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Defines a basic repository for an entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IRepository<T> where T : IAggregateRoot
    {
        T ById(Guid id);
        void Add(T toAdd);
        void Remove(T toRemove);
    }
}
