using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nydus.Fop.Filtering;

public static class FilterContextProcessor
{
    [Obsolete]
    public static IFilter ParseQuery(Type type, ResultExecutingContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var query = context.HttpContext.Request.Query;

        var queryStr = query["query"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(queryStr)) return null;

        return JsonSerializer.Deserialize(queryStr, type) as IFilter;
    }
}