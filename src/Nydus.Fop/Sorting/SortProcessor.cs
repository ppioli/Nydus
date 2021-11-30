using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nydus.Fop.Sorting;

public static class SortProcessor
{
    private static (Type declaringType, PropertyInfo property) GetPropertyInfoRecursively(
        this IQueryable queryable,
        string propName)
    {
        var nameParts = propName.Split('.');
        if (nameParts.Length == 1)
        {
            var property = queryable.ElementType.GetTypeInfo().GetProperty(CamelizeString(propName)) ??
                           queryable.ElementType.GetTypeInfo().GetProperty(propName);
            return (property?.DeclaringType, property);
        }

        //Getting Root Property - Ex : propName : "User.Name" -> User
        var propertyInfo = queryable.ElementType.GetTypeInfo().GetProperty(CamelizeString(nameParts[0])) ??
                           queryable.ElementType.GetTypeInfo().GetProperty(nameParts[0]);
        if (propertyInfo == null) return (null, null);

        var originalDeclaringType = propertyInfo.DeclaringType;
        for (var i = 1; i < nameParts.Length; i++)
        {
            propertyInfo = propertyInfo.PropertyType.GetProperty(CamelizeString(nameParts[i])) ??
                           propertyInfo.PropertyType.GetProperty(nameParts[i]);
            if (propertyInfo == null) return (null, null);
        }

        return (originalDeclaringType, propertyInfo);
    }

    private static LambdaExpression CreateExpression(Type type, string propertyName)
    {
        var param = Expression.Parameter(type, "v");
        Expression body = param;
        foreach (var member in propertyName.Split('.'))
            body = Expression.PropertyOrField(body, CamelizeString(member));

        return Expression.Lambda(body, param);
    }

    public static IQueryable ApplySorting(this IQueryable queryable, ICollection<SortOption> sortBy)
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));

        if (sortBy == null) throw new ArgumentNullException(nameof(sortBy));

        if (!sortBy.Any()) throw new Exception("Invalid parameter. Must provide at least one sort param.");

        var isFirst = true;
        foreach (var sortOption in sortBy)
        {
            var orderMethodName = SortMethodName(sortOption.IsDescending, isFirst);
            queryable = ApplySorting(queryable, orderMethodName, sortOption.PropertyName);
            isFirst = false;
        }

        return queryable;
    }

    private static IQueryable ApplySorting(IQueryable queryable, string sortMethodName, string propertyName)
    {
        var orderingProperty = GetPropertyInfoRecursively(queryable, propertyName);
        if (orderingProperty.declaringType == null || orderingProperty.property == null)
            return queryable;

        var orderByExp = CreateExpression(orderingProperty.declaringType, propertyName);
        if (orderByExp == null) return queryable;

        // If this is a nested expression, it should additionally add null checks to exclude null children
        //queryable = queryable.WrapInNullChecksIfAccessingNestedProperties(queryable.ElementType, propertyName);

        var wrappedExpression = Expression.Call(
            typeof(Queryable),
            sortMethodName,
            new[] { orderingProperty.declaringType, orderingProperty.property.PropertyType },
            queryable.Expression,
            Expression.Quote(orderByExp));
        var result = queryable.Provider.CreateQuery(wrappedExpression);
        return result;
    }

    private static IQueryable WrapInNullChecksIfAccessingNestedProperties(
        this IQueryable queryable,
        Type type,
        string propertyName)
    {
        var members = propertyName.Split('.');
        if (members.Length == 1) return queryable;

        // The following is essentially just appending a .Where() clause
        // to the queryable for each depth level of the query, e.g. for "Product.Data.Title"
        // it generates:
        // queryable
        //  .Where(x => x.Product != null)
        //  .Where(x => x.Product.Data != null)
        for (var i = 0; i < members.Length - 1; i++)
        {
            var member = members[i];
            var param = Expression.Parameter(type, "v");
            Expression body = param;
            for (var j = 0; j <= i; j++)
                body = Expression.PropertyOrField(body, CamelizeString(members[j])) ??
                       Expression.PropertyOrField(body, members[j]);

            var memberPath = members
                .TakeWhile((mem, index) => index <= i)
                .Aggregate((c, n) => c + "." + n);
            var notNullExpression = Expression.NotEqual(body, Expression.Constant(null));
            var notNullLambda = Expression.Lambda(notNullExpression, param);
            var whereMethodName = nameof(Queryable.Where);
            var nullCheckExpression = Expression.Call(
                typeof(Queryable),
                whereMethodName,
                new[] { type },
                queryable.Expression,
                Expression.Quote(notNullLambda));
            queryable = queryable.Provider.CreateQuery(nullCheckExpression);
        }

        return queryable;
    }

    private static string CamelizeString(string camelCase)
    {
        return camelCase.Substring(0, 1).ToUpperInvariant() + camelCase.Substring(1);
    }

    private static string SortMethodName(bool desc, bool isFirst)
    {
        if (isFirst) return desc ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
        return desc ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy);
    }
}