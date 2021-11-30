using System;
using System.Linq.Expressions;

namespace Nydus.EntityHelper;

public class MapperHelperOptions<TEntity>
{
    public Expression<Func<TEntity, bool>>? GlobalFilter { get; set; }
}