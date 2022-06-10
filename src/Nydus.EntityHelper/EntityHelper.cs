using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nydus.EntityHelper;

public class EntityHelper<TEntity, TUser, TUserKey, TDbContext> : MapperHelper<TEntity, TDbContext>,
    IEntityHelper<TEntity, TUser, TUserKey, TDbContext>
    where TEntity : class, IEntity<TUser, TUserKey>, new()
    where TUser : IdentityUser<TUserKey>
    where TUserKey : IEquatable<TUserKey>
    where TDbContext : DbContext
{
    private readonly TUser? _user;
    
    public override IQueryable<TEntity> Entities => base.Entities.Where(s => !s.SoftDeleted);

    protected EntityHelper(
        TDbContext dbContext,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        Func<HttpContext, TUser> getLoggedUser) : base(
        dbContext,
        mapper)
    {
        _user = contextAccessor.HttpContext == null ? null : getLoggedUser(contextAccessor.HttpContext!);
    }

    public override TEntity? TryFind(object id)
    {
        var find = base.TryFind(id);
        if (find == null || find.SoftDeleted) return null;
        return find;
    }

    protected override void HandleUpdate(TEntity e)
    {
        if (_user == null)
        {
            throw new Exception("No logged user attempting to modify entities");
        }

        e.UpdatedAt = DateTime.Now;
        e.UpdatedBy = _user;
        e.UpdatedById = _user.Id;
    }

    protected override void HandleCreate(TEntity e)
    {
        if (_user == null)
        {
            throw new Exception("No logged user attempting to modify entities");
        }

        e.UpdatedAt = DateTime.Now;
        e.UpdatedById = _user.Id;
        e.UpdatedBy = _user;
        e.CreatedAt = DateTime.Now;
        e.CreatedById = _user.Id;
        e.CreatedBy = _user;
    }

    public override void HandleDelete(TEntity e)
    {
        e.SoftDeleted = true;
    }
}