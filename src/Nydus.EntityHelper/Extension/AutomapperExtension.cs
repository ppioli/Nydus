using System;
using System.Linq.Expressions;
using AutoMapper;

namespace Nydus.EntityHelper.Extension;

public static class AutomapperExtension
{
    public static IMappingExpression<TSource, TDestination> MapFrom<TSource, TDestination, TMember>(
        this IMappingExpression<TSource, TDestination> exp,
        Expression<Func<TDestination, TMember>> destinationMember,
        Expression<Func<TSource, TMember>> func)
    {
        return exp.ForMember(destinationMember, opt => opt.MapFrom(func));
    }
}