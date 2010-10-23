using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Infrastructure.Web
{
    /// <summary>
    /// Base class for implementors of mvc contrib's pagination.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PaginationBase<T> : ISortedPagination<T>
    {
		public const int DefaultPageSize = 20;

        private int totalItems;
		public int PageSize { get; private set; }
        public int PageNumber { get; private set; }
        public string SortExpression { get; private set; }
        public SortDirection? SortDirection { get; private set; }

        public PaginationBase(
            int pageNumber,
            int pageSize,
            string sortExpression,
            SortDirection? sortDirection)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortExpression = sortExpression;
            SortDirection = sortDirection;
        }

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			TryExecuteQuery();

            return Results.GetEnumerator();
		}

		/// <summary>
		/// Executes the query if it has not already been executed.
        /// Must sets TotalItems and Results.
		/// </summary>
		protected abstract void TryExecuteQuery();

        public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

        protected IEnumerable<T> Results { get; set; }

		public int TotalItems
		{
			get
			{
				TryExecuteQuery();
				return totalItems;
			}
            protected set
            {
                totalItems = value;
            }
		}

		public int TotalPages
		{
			get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
		}

		public int FirstItem
		{
			get
			{
				TryExecuteQuery();
				return ((PageNumber - 1) * PageSize) + 1;
			}
		}

		public int LastItem
		{
			get
			{
				return FirstItem + Results.Count() - 1;
			}
		}

		public bool HasPreviousPage
		{
			get { return PageNumber > 1; }
		}

		public bool HasNextPage
		{
			get { return PageNumber < TotalPages; }
		}
    }
}
