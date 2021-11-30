using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Nydus.EntityHelper.KeyComparator;

public class EntityKeyFieldComparator<TEntity> : IEntityKeyComparator<TEntity>
{
    private readonly PropertyInfo _property;

    public EntityKeyFieldComparator(PropertyInfo property)
    {
        _property = property;
    }

    public Expression<Func<TEntity, bool>> EqualExpression(object b)
    {
        var value = GetValue(b);
        var param = Expression.Parameter(typeof(TEntity), _property.Name);
        var equalExp = Expression.Equal(
            Expression.Property(param, _property),
            Expression.Constant(value));

        return Expression.Lambda<Func<TEntity, bool>>(equalExp, param);
    }

    public Func<TEntity, bool> IsKeyEqual(object b)
    {
        var value = GetValue(b);

        return a => _property.GetValue(a) == value;
    }

    private object GetValue(object param)
    {
        var value = _property.GetValue(param);
        if (value == null) throw new Exception($"The parameter has no field {_property.Name}");

        return param;
    }
}