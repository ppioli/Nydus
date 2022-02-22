using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Nydus.EntityHelper;

public abstract class EntityHelper<TEntity, TUser, TUserKey> : MapperHelper<TEntity>, IEntityHelper<TEntity, TUser, TUserKey>
    where TEntity : class, IEntity<TUser, TUserKey>, new()
    where TUser : IdentityUser<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
    protected readonly IHttpContextAccessor ContextAccessor;
    
    public override IQueryable<TEntity> Entities => base.Entities.Where(s => !s.SoftDeleted);
    
    protected EntityHelper(DbContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor) : base(
        dbContext,
        mapper)
    {
        ContextAccessor = contextAccessor;
    }

    public override TEntity? TryFind(object id)
    {
        var find = base.TryFind(id);
        if (find == null || find.SoftDeleted) return null;
        return find;
    }

    protected override void HandleUpdate(TEntity e)
    {
        var user = GetLoggedUser();

        e.UpdatedAt = DateTime.Now;
        e.UpdatedBy = user;
        e.UpdatedById = user.Id;
    }

    protected override void HandleCreate(TEntity e)
    {
        var user = GetLoggedUser();
        e.UpdatedAt = DateTime.Now;
        e.UpdatedById = user.Id;
        e.UpdatedBy = user;
        e.CreatedAt = DateTime.Now;
        e.CreatedById = user.Id;
        e.CreatedBy = user;
    }

    public override void HandleDelete(TEntity e)
    {
        e.SoftDeleted = true;
    }

    protected HttpContext GetHttpContext()
    {
        if (ContextAccessor?.HttpContext == null) throw new Exception("The http context was not initialized on the entity helper");

        return ContextAccessor?.HttpContext!;
    }

    public abstract TUser GetLoggedUser();
}