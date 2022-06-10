using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nydus.EntityHelper;

public interface IEntityHelper<TEntity, TUser, TUserKey, out TDbContext> : IMapperHelper<TEntity, TDbContext>
    where TEntity : IEntity<TUser, TUserKey>, new()
    where TUser : IdentityUser<TUserKey>
    where TUserKey : IEquatable<TUserKey>
    where TDbContext : DbContext
{
}