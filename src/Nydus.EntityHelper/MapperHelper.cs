using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nydus.EntityHelper.Exceptions;

namespace Nydus.EntityHelper;

public class MapperHelper<TEntity, TDbContext> : IMapperHelper<TEntity, TDbContext> 
    where TEntity : class, new()
    where TDbContext: DbContext 
{
    private readonly IConfigurationProvider _config;
    private readonly DbSet<TEntity> _dbSet;
    public IMapper Mapper { get; }
    private readonly IKey _primaryKey;
    public TDbContext DbContext { get; }
    private IQueryable<TEntity> _queryable;
    
    public virtual IQueryable<TEntity> Entities => _queryable;
    
    public MapperHelper(TDbContext dbContext, IMapper mapper, MapperHelperOptions<TEntity>? options = null)
    {
        DbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
        _queryable = _dbSet;
        Mapper = mapper;
        _config = mapper.ConfigurationProvider;
        
        _primaryKey = dbContext.Model
            .FindEntityType(typeof(TEntity))?
            .FindPrimaryKey() ?? throw new Exception($"Could not find primary key for Entity {nameof(TEntity)}");
        
        SetOptions(options);
    }

    public void SetOptions(MapperHelperOptions<TEntity>? options = null)
    {
        _queryable = options?.QueryableBuilder != null ? options.QueryableBuilder(_dbSet) : _dbSet;
    }

    public IQueryable<TDto> Dtos<TDto>()
    {
        return Entities
            .UseAsDataSource(_config)
            .For<TDto>();
    }

    public async Task<TDto> Create<TDto, TCreate>(TCreate model, TEntity? created = null, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null)
    {
        var entity = opts != null ? CreateEntity(model, created, opts ) : CreateEntity(model, created);

        _dbSet.Attach(entity);

        await DbContext.SaveChangesAsync();

        return Mapper.Map<TDto>(entity);
    }

    public Task<TDto> Create<TDto>(TDto model, TEntity? created = null, Action<IMappingOperationOptions<TDto, TEntity>>? opts = null)
    {
        return Create<TDto, TDto>(model, created, opts);
    }
    
    public async Task<IEnumerable<TDto>> CreateBatch<TDto>(ICollection<TDto> model, Action<IMappingOperationOptions<TDto, TEntity>>? opts = null)
    {
        var created = new List<TEntity>();

        foreach (var item in model)
        {
            var entity = new TEntity();

            if (opts != null)
            {
                Mapper.Map(item, entity, opts);    
            }
            else
            {
                Mapper.Map(item, entity);
            }

            HandleCreate(entity);

            created.Add(entity);
        } 
        
        _dbSet.AttachRange(created);

        await DbContext.SaveChangesAsync();

        return model;
    }

    public TEntity CreateEntity<TCreate>(TCreate model, TEntity? created = default, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null)
    {
        var entity = created ?? new TEntity();

        if (opts != null)
        {
            Mapper.Map(model, entity, opts);    
        }
        else
        {
            Mapper.Map(model, entity);
        }

        HandleCreate(entity);

        return entity;
    }

    public TEntity UpdateEntityById<TCreate>(TCreate model, object id, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null)
    {
        var entity = Find(id);
        
        HandleUpdate(entity);

        if (opts != null)
        {
            Mapper.Map(model, entity, opts);    
        }
        else
        {
            Mapper.Map(model, entity);
        }
        

        return entity;
    }
    
    public async Task<TDto> Update<TDto, TCreate>(TCreate model, TEntity entity, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null)
    {
        UpdateEntity(model, entity, opts);

        await DbContext.SaveChangesAsync();

        _dbSet.Attach(entity);

        await DbContext.SaveChangesAsync();

        return Mapper.Map<TDto>(entity);
    }
    
    public TEntity UpdateEntity<TCreate>(TCreate model, TEntity entity, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null)
    {
        HandleUpdate(entity);

        if (opts != null)
        {
            Mapper.Map(model, entity, opts);    
        }
        else
        {
            Mapper.Map(model, entity);
        }

        return entity;
    }
    
    public Task<TDto> Update<TDto, TCreate>(TCreate model, object id, Action<IMappingOperationOptions<TCreate, TEntity>>? opts = null)
    {
        var entity = Find(id);

        return Update<TDto, TCreate>(model, entity, opts);
    }
    
    public Task<TDto> Update<TDto>(TDto model, TEntity entity, Action<IMappingOperationOptions<TDto,TEntity>>? opts = null)
    {
        return Update<TDto, TDto>(model, entity, opts);
    }

    public Task<TDto> Update<TDto>(TDto model, object id, Action<IMappingOperationOptions<TDto, TEntity>>? opts = null)
    {
        var entity = Find(id);

        return Update<TDto, TDto>(model, entity, opts);
    }
    

    public TDto? Get<TDto>(object id)
    {
        var entity = TryFind(id);

        return entity == null ? default : Mapper.Map<TDto>(entity);
    }

    public IEnumerable<TDto> Project<TDto>( IQueryable<TEntity> queryable )
    {
        return Mapper.ProjectTo<TDto>(queryable);
    }

    public async Task Delete(object o)
    {
        var deleted = await FindAsync(o);

        HandleDelete(deleted);

        await DbContext.SaveChangesAsync();
    }

    public TDto ToDto<TDto>(TEntity entity, Action<IMappingOperationOptions<TEntity, TDto>>? opts = null)
    {
        
        return opts != null ? Mapper.Map(entity, opts) : Mapper.Map<TDto>(entity);    
        
    }

    public IQueryable<TDto> ToDto<TDto>(IQueryable<TEntity> entities)
    {
        return Mapper.ProjectTo<TDto>(entities);
    }

    public virtual void HandleUpdate(TEntity e)
    {
    }

    public virtual void HandleCreate(TEntity e)
    {
    }

    public virtual void HandleDelete(TEntity e)
    {
        DbContext.Remove(e);
    }
    
    public virtual TEntity? TryFind(object id)
    {
        return _dbSet.Find(id);
    }
    
    private async Task<TEntity?> TryFindAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }
    
    private async Task<TEntity> FindAsync(object id)
    {
        return await TryFindAsync(id) ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }

    private TEntity Find(object id)
    {
        return TryFind(id) ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }


    public object KeyOf(TEntity entity)
    {
        var keyProperties = _primaryKey.Properties.Select(s => s.Name);

        var keyParts = keyProperties
            .Select(propertyName => GetPropertyValue(entity, propertyName))
            .ToArray();

        if (keyParts.Length == 1) return keyParts[0];

        throw new NotImplementedException();
    }


    private object GetPropertyValue(TEntity entity, string name)
    {
        return entity.GetType().GetProperty(name)!.GetValue(entity, null) ??
               throw new Exception("Could not find key value");
    }
}