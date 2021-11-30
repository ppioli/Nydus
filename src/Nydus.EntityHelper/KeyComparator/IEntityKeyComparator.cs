using System;
using System.Linq.Expressions;

namespace Nydus.EntityHelper.KeyComparator;

public interface IEntityKeyComparator<TA>
{
    Expression<Func<TA, bool>> EqualExpression(object b);
    Func<TA, bool> IsKeyEqual(object b);
}