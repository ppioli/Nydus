using System.Linq;
using Nydus.Fop.Filtering;

namespace Nydus.Fop.Extension;

public static class QueryableExtension
{
    public static IQueryable ApplyFilter(this IQueryable queryable, IFilter filter)
    {
        return filter.Apply(queryable);
    }

    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, IFilter<T> filter)
    {
        return filter.Apply(queryable);
    }
}