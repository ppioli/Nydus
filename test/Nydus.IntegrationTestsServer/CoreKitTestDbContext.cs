using Microsoft.EntityFrameworkCore;

namespace Nydus.IntegrationTestsServer;

public class CoreKitTestDbContext : DbContext
{
    public CoreKitTestDbContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<User> Users { get; set; }
}