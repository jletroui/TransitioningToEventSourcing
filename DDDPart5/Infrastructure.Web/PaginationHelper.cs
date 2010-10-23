using System;

namespace Infrastructure.Web
{
    /// <summary>
    /// Extension methods for creating paged lists.
    /// Note: this class is inspired by MvcContrib.Pagination.PaginationHelper.
    /// </summary>
    public static class PaginationHelper
    {
        public static ISortedPagination<T> AsPagination<T>(this IPaginable<T> source, int pageNumber)
        {
            return AsPagination<T>(source, pageNumber, PaginablePagination<T>.DefaultPageSize, null, null);
        }

        public static ISortedPagination<T> AsPagination<T>(this IPaginable<T> source, int pageNumber, int pageSize)
        {
            return AsPagination<T>(source, pageNumber, pageSize, null, null);
        }

        public static ISortedPagination<T> AsPagination<T>(this IPaginable<T> source, 
            int pageNumber, 
            int pageSize,
            string sortExpression,
            SortDirection? sortDirection)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
            }

            return new PaginablePagination<T>(source, pageNumber, pageSize, sortExpression, sortDirection);
        }

    }
}
