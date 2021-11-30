using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Nydus.Fop.Extension;

namespace Nydus.Fop.Annotations;

public class FilteredAttribute : ActionFilterAttribute, ICoreKitAttribute
{
    public FilteredAttribute()
    {
        Order = (int)ActionOrder.Filtering;
    }

    private bool IsFilterParameter(ParameterDescriptor param)
    {
        return param.ParameterType.IsFilterType();
    }
}