using System.Linq;
using Nydus.Fop.Filtering;

namespace Nydus.IntegrationTestsServer.Filters;

public class UsernameFilter : IFilter<User>
{
    public string UserName { get; set; }

    public IQueryable<User> Apply(IQueryable<User> queryable)
    {
        if (UserName != null) queryable = queryable.Where(u => u.UserName.Contains(UserName));

        return queryable;
    }
}