using System;
using System.Linq;

namespace Nydus.Fop.Pagination;

public static class PaginationProcessor
{
    public static PageResult<object> AsPageResult(this IQueryable queryable, PaginationOptions options)
    {
        if (options.PageSize < 1) throw new Exception($"Invalid page size {options.PageSize}");

        if (options.Page < 1) throw new Exception($"Invalid page {options.Page}. Pages starts at 1");
        dynamic dynamicQuery = queryable;
        var totalCount = Queryable.Count(dynamicQuery);
        var result = Queryable.Take(
            Queryable.Skip(dynamicQuery, (options.Page - 1) * options.PageSize),
            options.PageSize);

        return new PageResult<object>
        {
            Page = options.Page,
            PageSize = options.PageSize,
            TotalCount = totalCount,
            Content = ((IQueryable<object>)result).ToList(),
        };
    }
}