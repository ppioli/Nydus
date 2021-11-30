using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nydus.Fop.Sorting;

public static class SortingContextProcessor
{
    public static ICollection<SortOption> ParseSorting(ResultExecutingContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var query = context.HttpContext.Request.Query;
        var sortParameters = query["sort"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(sortParameters)) return null;
        var splittedParameters = Regex.Matches(sortParameters, @"[+-]{1}[a-zA-Z0-9_\.]{1,}")
            .Select(m => m.Value)
            .ToArray();

        if (!splittedParameters.Any())
            // todo fix this message
            throw new Exception(
                "Invalid sort parameters. Sort parameters must be a string of field to sort by, " +
                "each starting by a plus sign (+) or a minus sign (-) depending on the directing you want to sort by");

        var result = new List<SortOption>();
        foreach (var param in splittedParameters)
        {
            if (param.Length <= 2)
                throw new Exception($"Invalid sort param='{param}'. Must be at least two characters long");

            var desc = param[0] switch
            {
                '+' => false,
                '-' => true,
                _ => throw new Exception($"Invalid sort param='{param}'. Must start with +/-"),
            };

            result.Add(
                new SortOption
                {
                    IsDescending = desc,
                    PropertyName = param.Substring(1),
                });
        }

        return result;
    }
}