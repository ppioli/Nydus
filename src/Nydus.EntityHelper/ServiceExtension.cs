using System;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nydus.EntityHelper.Collection;

namespace Nydus.EntityHelper;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds automapper and the mapper helpers. (Also add the DbContext to the DI system)
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TStarUpAssembly"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddMapperHelper<TDbContext, TStarUpAssembly>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddAutoMapper(
            (serviceProvider, automapper) =>
            {
                automapper.AddMaps(typeof(TStarUpAssembly));
                automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<TDbContext>(serviceProvider);
            },
            typeof(TDbContext).Assembly);

        services.AddScoped<DbContext, TDbContext>();
        services.AddScoped(typeof(IMapperHelper<,>), typeof(MapperHelper<,>));

        return services;
    }

    // public interface IEntityHelperFactory
    // {
    //     IEntityHelper<TEntity, TUser, TRol> Get<TEntity, TUser, TRol>(MapperHelperOptions<TEntity>? options)
    //         where TEntity : class, IEntity<TUser, TRol>, new()
    //         where TUser : IdentityUser<TRol>
    //         where TRol : IEquatable<TRol>;
    //
    //     IEntityHelper<TEntity, TUser, TRol> Get<TEntity, TUser, TRol>()
    //         where TEntity : class, IEntity<TUser, TRol>, new()
    //         where TUser : IdentityUser<TRol>
    //         where TRol : IEquatable<TRol>;
    // }
}
    