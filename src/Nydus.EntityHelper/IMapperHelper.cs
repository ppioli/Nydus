using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Nydus.EntityHelper;

public interface IMapperHelper<TEntity, out TDbContext>
where TDbContext : DbContext
{
    TDbContext DbContext { get; }
    IQueryable<TEntity> Entities { get; }
    IQueryable<TDto> Dtos<TDto>();
    Task<TDto> Create<TDto, TCreate>(TCreate model, TEntity? created = default);
    Task<TDto> Create<TDto>(TDto model, TEntity? created = default);
    Task<IEnumerable<TDto>> CreateBatch<TDto>(IEnumerable<TDto> model);
    Task<TDto> Update<TDto, TCreate>(TCreate model, object id);
    Task<TDto> Update<TDto>(TDto model, object id);
    Task<TDto> Update<TDto, TCreate>(TCreate model, TEntity entity);
    Task<TDto> Update<TDto>(TDto model, TEntity entity);
    TEntity? TryFind(object id);
    TDto? Get<TDto>(object id);
    Task Delete(object o);
    TDto ToDto<TDto>(TEntity user, Action<IMappingOperationOptions> opts);
    IQueryable<TDto> ToDto<TDto>(IQueryable<TEntity> entities);
    void SetOptions(MapperHelperOptions<TEntity> options);
}