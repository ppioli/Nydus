using System;
using Nydus.Fop.Filtering;

namespace Nydus.Fop.Extension;

public static class TypeExtension
{
    public static bool IsFilterType(this Type t)
    {
        return typeof(IFilter).IsAssignableFrom(t);
    }
}