using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nydus.IntegrationTestsServer;

public class IntegrationTestServer
{
    private static TestServer _testServer;
    private static readonly object _initializationLock = new();

    public static TestServer GetTestServer()
    {
        if (_testServer == null) InitializeTestServer();
        return _testServer;
    }

    public static CoreKitTestDbContext GetNewContext()
    {
        using (var serviceScope = GetTestServer().Host.Services.CreateScope())
        {
            var serviceProvider = serviceScope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<CoreKitTestDbContext>();
            return context;
        }
    }

    private static void InitializeTestServer()
    {
        lock (_initializationLock)
        {
            if (_testServer != null) return;
            var webHostBuilder = new WebHostBuilder()
                .ConfigureLogging(logging => logging.AddDebug())
                .UseStartup<Startup>();
            var testServer = new TestServer(webHostBuilder);
            var initializationTask = InitializeDatabase(testServer);
            initializationTask.ConfigureAwait(false);
            initializationTask.Wait();
            _testServer = testServer;
        }
    }

    private static async Task InitializeDatabase(TestServer testServer)
    {
        using (var serviceScope = testServer.Host.Services.CreateScope())
        {
            var serviceProvider = serviceScope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<CoreKitTestDbContext>();
            var initializer = new DatabaseInitializer(context);
            await initializer.InitializeDatabase();
        }
    }
}