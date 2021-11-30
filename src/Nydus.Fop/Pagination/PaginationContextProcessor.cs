using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nydus.Fop.Pagination;

public static class PaginationContextProcessor
{
    // TODO reorganize this
    public static PaginationOptions ParsePagination(
        ResultExecutingContext context,
        int defaultPageSize,
        int maxPageSize)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var query = context.HttpContext.Request.Query;
        defaultPageSize = Math.Min(defaultPageSize, maxPageSize);

        var result = new PaginationOptions
        {
            Page = 1,
            PageSize = defaultPageSize,
        };

        var pageParam = query["page"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(pageParam) && int.TryParse(pageParam, out var parsedPage))
            result.Page = Math.Max(1, parsedPage);

        var pageSizeParam = query["pageSize"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(pageSizeParam) && int.TryParse(pageSizeParam, out var parsedPageSize))
        {
            var size = parsedPageSize > 0
                ? parsedPageSize
                : defaultPageSize;
            result.PageSize = Math.Min(size, maxPageSize);
        }

        return result;
    }
}