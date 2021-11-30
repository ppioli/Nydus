using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Nydus.Fop.Annotations;
using Nydus.Fop.Pagination;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nydus.Fop.Swashbuckle;

/// <summary>
///     Generates operation filter for Nydus.Fop.Annotations' parameters
///     Swashbuckle <see cref="IOperationFilter" />
/// </summary>
/// <remarks>OpenAPI Support</remarks>
public class CoreKitOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var action = (ControllerActionDescriptor)context.ApiDescription.ActionDescriptor;

        if (action.MethodInfo.GetCustomAttributes().Any(a => a is SortedAttribute))
            // sort
            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "sort",
                    In = ParameterLocation.Query,
                    Description = "sort",
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("+id-descripcion"),
                    },
                });

        if (action.MethodInfo.GetCustomAttributes().Any(a => a is PaginatedAttribute))
        {
            var responseType = action.MethodInfo.ReturnType;
            if (!responseType.IsAssignableTo(typeof(IQueryable<object>)))
                //TODO Improve errors
                throw new Exception("Error");

            // pageSize
            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "pageSize",
                    In = ParameterLocation.Query,
                    Description = "page size",
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Example = new OpenApiInteger(10),
                    },
                });

            // page
            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "page",
                    In = ParameterLocation.Query,
                    Description = "page",
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Example = new OpenApiInteger(1),
                    },
                });

            var baseType = responseType.GenericTypeArguments[0];

            var pageType = typeof(PageResult<>).MakeGenericType(baseType);

            var newSchemaId = $"{baseType.Name}PageResult";
            if (!context.SchemaRepository.TryLookupByType(pageType, out var schema))
            {
                if (context.SchemaRepository.Schemas.ContainsKey(newSchemaId))
                    throw new Exception($"An schema with id {newSchemaId} already exist");
                schema = context.SchemaGenerator.GenerateSchema(pageType, context.SchemaRepository);
            }


            var schemaType = new OpenApiMediaType
            {
                Schema = schema,
            };

            operation.Responses[StatusCodes.Status200OK.ToString()].Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = schemaType,
                ["text/json"] = schemaType,
                ["plain/text"] = schemaType,
            };
        }
    }
}