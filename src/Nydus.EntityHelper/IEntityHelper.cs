using System;
using Microsoft.AspNetCore.Identity;

namespace Nydus.EntityHelper;

public interface IEntityHelper<TEntity, TUser, TUserKey> : IMapperHelper<TEntity>
    where TEntity : IEntity<TUser, TUserKey>, new()
    where TUser : IdentityUser<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
}