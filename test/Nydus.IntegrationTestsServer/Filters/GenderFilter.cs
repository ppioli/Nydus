using System.Linq;
using Nydus.Fop.Filtering;

namespace Nydus.IntegrationTestsServer.Filters;

public class GenderFilter : IFilter<User>
{
    public bool? IsMale { get; set; }

    public IQueryable<User> Apply(IQueryable<User> queryable)
    {
        if (IsMale != null) queryable = queryable.Where(u => u.Gender == (IsMale.Value ? "M" : "F"));

        return queryable;
    }
}