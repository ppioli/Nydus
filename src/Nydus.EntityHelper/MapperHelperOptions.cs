using System;
using System.Linq;
using System.Linq.Expressions;

namespace Nydus.EntityHelper;

public class MapperHelperOptions<TEntity>
{
    public Func<IQueryable<TEntity>, IQueryable<TEntity>>? QueryableBuilder { get; set; }
}