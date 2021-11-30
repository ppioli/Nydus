using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nydus.Fop.Sorting;

namespace Nydus.Fop.Annotations;

public class SortedAttribute : ActionFilterAttribute, ICoreKitAttribute
{
    public SortedAttribute()
    {
        Order = (int)ActionOrder.Sorting;
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        var objectResult = context.Result as ObjectResult;

        if (!(objectResult?.Value is IQueryable queryable))
            // TODO improve this error message
            throw new Exception("Invalid things");

        var sortOptions = SortingContextProcessor.ParseSorting(context);

        if (sortOptions == null) return;

        objectResult.Value = queryable.ApplySorting(sortOptions);
    }
}