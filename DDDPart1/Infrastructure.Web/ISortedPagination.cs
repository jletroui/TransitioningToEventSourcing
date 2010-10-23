using MvcContrib.Pagination;

namespace Infrastructure.Web
{
    /// <summary>
    /// A pagination that also has support for sorting.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISortedPagination<T> : IPagination<T>
    {
        string SortExpression { get; }
        SortDirection? SortDirection { get; }
    }
}
