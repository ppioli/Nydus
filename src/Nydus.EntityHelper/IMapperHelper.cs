using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Nydus.EntityHelper;

public interface IMapperHelper<TEntity>
{
    IQueryable<TEntity> Entities { get; }
    IQueryable<TDto> Dtos<TDto>();
    Task<TDto> Create<TDto, TCreate>(TCreate model, TEntity created);
    Task<TDto> Update<TDto, TCreate>(TCreate model, object id);
    Task<TDto> Update<TDto, TCreate>(TCreate model, TEntity entity);
    TEntity? TryFind(object id);
    TDto? Get<TDto>(object id);
    Task Delete(object o);
    TDto ToDto<TDto>(TEntity user, Action<IMappingOperationOptions> opts);
    IQueryable<TDto> ToDto<TDto>(IQueryable<TEntity> entities);
}