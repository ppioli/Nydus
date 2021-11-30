using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nydus.EntityHelper;

public static class QueryHelper
{
    private static readonly MethodInfo OrderByMethod =
        typeof(Queryable).GetMethods()
            .Single(
                method =>
                    method.Name == "OrderBy" && method.GetParameters().Length == 2);

    private static readonly MethodInfo ThenOrderByMethod =
        typeof(Queryable).GetMethods()
            .Single(
                method =>
                    method.Name == "ThenBy" && method.GetParameters().Length == 2);

    private static readonly MethodInfo OrderByDescendingMethod =
        typeof(Queryable).GetMethods()
            .Single(
                method =>
                    method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

    private static readonly MethodInfo ThenOrderByDescendingMethod =
        typeof(Queryable).GetMethods()
            .Single(
                method =>
                    method.Name == "ThenByDescending" && method.GetParameters().Length == 2);

    public static IQueryable<T> OrderByProperty<T>(
        this IQueryable<T> source,
        string propertyName)
    {
        var paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        var lambda = Expression.Lambda(orderByProperty, paramterExpression);
        var genericMethod =
            OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        var ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }

    public static IQueryable<T> ThenOrderByProperty<T>(
        this IQueryable<T> source,
        string propertyName)
    {
        var paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        var lambda = Expression.Lambda(orderByProperty, paramterExpression);
        var genericMethod =
            ThenOrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        var ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }

    public static IQueryable<T> OrderByPropertyDescending<T>(
        this IQueryable<T> source,
        string propertyName)
    {
        var paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        var lambda = Expression.Lambda(orderByProperty, paramterExpression);
        var genericMethod =
            OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        var ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }

    public static IQueryable<T> ThenOrderByPropertyDescending<T>(
        this IQueryable<T> source,
        string propertyName)
    {
        var paramterExpression = Expression.Parameter(typeof(T));
        Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
        var lambda = Expression.Lambda(orderByProperty, paramterExpression);
        var genericMethod =
            ThenOrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
        var ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)ret;
    }
}