using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Nydus.IntegrationTestsServer.Controllers;

[Route("Values")]
public class ValuesController : Controller
{
    private readonly CoreKitTestDbContext _context;

    public ValuesController(CoreKitTestDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetValues()
    {
        var users = _context.Users.OrderBy(u => Guid.NewGuid());
        return Ok(users);
    }
}