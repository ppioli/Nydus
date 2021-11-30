using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nydus.Fop.Annotations;

namespace Nydus.IntegrationTestsServer.Controllers;

public class SortedPaginatedController
{
    private readonly CoreKitTestDbContext _context;

    public SortedPaginatedController(CoreKitTestDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("/SortedPaginated")]
    [Paginated(4)]
    [Sorted]
    public IQueryable<User> GetValues()
    {
        return _context.Users.AsQueryable();
    }
}