using System;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure
{
    /// <summary>
    /// Defines a datasource that is paginable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPaginable<T>
    {
        int Count();
        T UniqueValue();
        IEnumerable<T> ToEnumerable();
        IEnumerable<T> ToEnumerable(int skip, int take);
        IEnumerable<T> ToEnumerable(int skip, int take, string sortColumn, SortDirection? sortDirection);
    }

    /// <summary>
    /// This enum is a platform-independent sort direction representation.
    /// </summary>
    [Serializable]
    public enum SortDirection
    {
        Asc,
        Desc
    }
}
