using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nydus.Fop.Annotations;

namespace Nydus.IntegrationTestsServer.Controllers;

public class SortedController
{
    private readonly CoreKitTestDbContext _context;

    public SortedController(CoreKitTestDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("/Sorted")]
    [Sorted]
    public IQueryable<User> GetValues()
    {
        // Mix users up to ensure that sorting actually works
        var users = _context.Users
            .Include(u => u.FavoriteAnimal)
            .OrderBy(u => Guid.NewGuid())
            .ToList();

        return users.AsQueryable();
    }
}