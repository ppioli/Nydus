using System;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nydus.EntityHelper.Collection;

namespace Nydus.EntityHelper;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMapperHelper<TDbContext, TStarUpAssembly>(this IServiceCollection services) 
        where TDbContext : DbContext 
    {
        services.AddAutoMapper((serviceProvider, automapper) =>
        {
            automapper.AddMaps(typeof(TStarUpAssembly));

            automapper.AddCollectionMappers();
            automapper.UseEntityFrameworkCoreModel<TDbContext>(serviceProvider);
        }, typeof(TDbContext).Assembly);

        return services;
    }
}