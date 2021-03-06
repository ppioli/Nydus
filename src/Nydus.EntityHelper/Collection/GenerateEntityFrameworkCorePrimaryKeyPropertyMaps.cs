using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Nydus.EntityHelper.Collection;

public class GenerateEntityFrameworkCorePrimaryKeyPropertyMaps<TDatabaseContext> : IGeneratePropertyMaps
    where TDatabaseContext : DbContext
{
    private readonly IModel _model;

    public GenerateEntityFrameworkCorePrimaryKeyPropertyMaps()
    {
        throw new InvalidOperationException(
            $"Use {nameof(MapperConfigurationExpressionExtensions.UseEntityFrameworkCoreModel)} instead of using SetGeneratePropertyMaps.");
    }

    public GenerateEntityFrameworkCorePrimaryKeyPropertyMaps(IModel model)
    {
        _model = model;
    }

    public IEnumerable<PropertyMap> GeneratePropertyMaps(TypeMap typeMap)
    {
        var propertyMaps = typeMap.PropertyMaps;
        var keyMembers = _model.FindEntityType(typeMap.DestinationType)?.FindPrimaryKey().Properties ??
                         new List<IProperty>();
        return keyMembers.Select(m => propertyMaps.FirstOrDefault(p => p.DestinationMember.Name == m.Name));
    }
}