using System;
using Microsoft.AspNetCore.Identity;

namespace Nydus.EntityHelper;

public interface IEntity<TUser, TKey> where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
{
    public TUser UpdatedBy { get; set; }
    public TKey UpdatedById { get; set; }
    public TUser CreatedBy { get; set; }
    public TKey CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool SoftDeleted { get; set; }
}