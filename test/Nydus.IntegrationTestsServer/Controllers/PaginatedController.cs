using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nydus.Fop.Annotations;

namespace Nydus.IntegrationTestsServer.Controllers;

public class PaginatedController
{
    private readonly CoreKitTestDbContext _context;

    public PaginatedController(CoreKitTestDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("/Paginated")]
    [Paginated(4, 7)]
    public IQueryable<User> GetValues()
    {
        return _context.Users.AsQueryable();
    }
}