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
    
    Task<TDto> Create<TDto, TCreate>(TCreate model, TEntity? created = default, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null);
    Task<TDto> Create<TDto>(TDto model, TEntity? created = default, Action<IMappingOperationOptions<TDto, TEntity>>? opts = null);
    TEntity CreateEntity<TCreate>(TCreate model, TEntity? created = default, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null);
    Task<IEnumerable<TDto>> CreateBatch<TDto>(ICollection<TDto> model, Action<IMappingOperationOptions<TDto, TEntity>>? opts = null);
    
    TEntity UpdateEntityById<TCreate>(TCreate model, object id, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null);
    TEntity UpdateEntity<TCreate>(TCreate model, TEntity entity, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null);
    
    Task<TDto> Update<TDto, TCreate>(TCreate model, object id, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null);
    
    Task<TDto> Update<TDto>(TDto model, object id, Action<IMappingOperationOptions<TDto, TEntity>>? opts = null);
    Task<TDto> Update<TDto, TCreate>(TCreate model, TEntity entity, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null);
    Task<TDto> Update<TDto>(TDto model, TEntity entity, Action<IMappingOperationOptions<TDto, TEntity>>? opts = null);
    TEntity? TryFind(object id);
    TDto? Get<TDto>(object id);
    Task Delete(object o);
    TDto ToDto<TDto>(TEntity user, Action<IMappingOperationOptions<TEntity,TDto>> opts);
    IQueryable<TDto> ToDto<TDto>(IQueryable<TEntity> entities);
    void SetOptions(MapperHelperOptions<TEntity> options);
}