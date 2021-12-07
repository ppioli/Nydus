using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nydus.EntityHelper.Extension;

namespace Nydus.EntityHelper;

public class EntityHelper<TEntity, TUser, TUserKey> : MapperHelper<TEntity>, IEntityHelper<TEntity, TUser, TUserKey>
    where TEntity : class, IEntity<TUser, TUserKey>, new()
    where TUser : IdentityUser<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
    private readonly IHttpContextAccessor _contextAccessor;
    
    public override IQueryable<TEntity> Entities => base.Entities.Where(s => !s.SoftDeleted);
    
    protected EntityHelper(DbContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor) : base(
        dbContext,
        mapper)
    {
        _contextAccessor = contextAccessor;
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

    public HttpContext GetHttpContext()
    {
        if (_contextAccessor?.HttpContext == null) throw new Exception("The http context was not initialized on the entity helper");

        return _contextAccessor?.HttpContext!;
    }

    public TUser GetLoggedUser()
    {
        var userId = GetHttpContext().User.GetId<TUserKey>();
        if (userId == null) throw new Exception("Could not get id for the current user");

        var user = DbContext.Set<TUser>().Find(userId);

        if (user == null) throw new Exception("Could not get logged user");

        return user;
    }
}