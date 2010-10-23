using System.Collections.Generic;
using System.Collections;

namespace Infrastructure.Web
{
    /// <summary>
    /// Pagination on a Paginable.
    /// Note: this class is inspired by MvcContrib.Pagination.LazyPagination that do a similar thing on a queryable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginablePagination<T> : PaginationBase<T>
    {
		public IPaginable<T> Query { get; private set; }

        public PaginablePagination(IPaginable<T> query,
            int pageNumber,
            int pageSize,
            string sortExpression,
            SortDirection? sortDirection) :
            base(pageNumber, pageSize, sortExpression, sortDirection)
        {
            Query = query;
        }

		protected override void TryExecuteQuery()
		{
            if (Results != null)
            {
                return;
            }

			TotalItems = Query.Count();

            int numberToSkip = (PageNumber - 1) * PageSize;

            Results = Query.ToEnumerable(
                numberToSkip,
                PageSize,
                SortExpression,
                SortDirection);
        }
    }
}
