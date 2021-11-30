using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nydus.Fop.Annotations;
using Nydus.Fop.Extension;
using Nydus.IntegrationTestsServer.Filters;

namespace Nydus.IntegrationTestsServer.Controllers;

public class FilteredController
{
    private readonly CoreKitTestDbContext _context;

    public FilteredController(CoreKitTestDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("/Filtered")]
    [Filtered]
    public IQueryable<User> GetValuesCombined([FromQuery] GenderFilter gender)
    {
        return _context.Users.AsQueryable().ApplyFilter(gender);
    }
}