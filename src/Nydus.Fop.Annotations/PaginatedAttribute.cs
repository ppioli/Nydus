using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nydus.Fop.Pagination;

namespace Nydus.Fop.Annotations;

public class PaginatedAttribute : ActionFilterAttribute, ICoreKitAttribute
{
    private readonly int _defaultPageSize;
    private readonly int _maxPageSize;

    public PaginatedAttribute(int defaultPageSize = 20, int maxPageSize = 50)
    {
        Order = (int)ActionOrder.Paginating;
        _maxPageSize = maxPageSize;
        _defaultPageSize = defaultPageSize;
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var objectResult = context.Result as ObjectResult;

        if (!(objectResult?.Value is IQueryable queryable))
            // TODO improve this error message
            throw new Exception("Invalid things");

        var paginationOptions = PaginationContextProcessor.ParsePagination(
            context,
            _defaultPageSize,
            _maxPageSize);


        objectResult.Value = queryable.AsPageResult(paginationOptions);
    }
}