using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nydus.Fop.Swashbuckle;

namespace Nydus.IntegrationTestsServer;

public class Startup
{
    private static readonly string InMemoryDatabaseName = Guid.NewGuid().ToString();

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CoreKitTestDbContext>(
            options =>
                options
                    .UseInMemoryDatabase(InMemoryDatabaseName));

        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc(
                    "swagger20",
                    new OpenApiInfo
                    {
                        Description = "swagger20",
                    });
                options.OperationFilter<CoreKitOperationFilter>();
            });

        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc(
                    "openapi30",
                    new OpenApiInfo
                    {
                        Description = "openapi30",
                    });
                options.OperationFilter<CoreKitOperationFilter>();
            });

        services.AddMvc(mvcOptions => mvcOptions.EnableEndpointRouting = false);
    }

    public void Configure(IApplicationBuilder app)
    {
        /*
         * Change the path for swageer json endpoints
         * https://github.com/domaindrivendev/Swashbuckle.AspNetCore#change-the-path-for-swagger-json-endpoints
         */
        app.UseSwagger(
            options =>
            {
                options.RouteTemplate = "swashbuckle/{documentName}.json";
                options.SerializeAsV2 = true;
            });

        app.UseSwagger(options => { options.RouteTemplate = "swashbuckle/{documentName}.json"; });

        app.UseMvc();
    }
}