using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nydus.EntityHelper.KeyComparator;

public class KeyComparatorBuilder
{
    private readonly DbContext _dbContext;

    public KeyComparatorBuilder(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEntityKeyComparator<TEntity> GetComparator<TEntity, TModel>()
    {
        var key = _dbContext.Model
            .FindEntityType(typeof(TEntity))
            .FindPrimaryKey();

        if (key == null || key.Properties.Count == 0)
            throw new Exception($"Found unexpected value. {typeof(TEntity).Name} has no primary key");

        // check that TEntity y TCreate types are compatible
        var comparators = new List<IEntityKeyComparator<TEntity>>();

        foreach (var keyProperty in key.Properties)
        {
            var a = keyProperty.PropertyInfo;
            if (!(a.PropertyType.IsPrimitive || a.PropertyType == typeof(string)))
                throw new Exception("Only keys with primitives values are supported");

            if (typeof(TModel) != typeof(object))
            {
                var b = typeof(TModel).GetProperty(a.Name);
                if (b == null) throw new Exception($"Key Field {a.Name} is not included in {typeof(TModel).Name}");

                if (a.PropertyType != b.PropertyType) throw new Exception($"Key Field {a.Name} types mismatch");
            }

            comparators.Add(new EntityKeyFieldComparator<TEntity>(a));
        }

        return comparators.Count == 1 ? comparators[0] : throw new NotImplementedException();
    }

    public IEntityKeyComparator<TEntity> GetComparator<TEntity>()
    {
        return GetComparator<TEntity, object>();
    }
}