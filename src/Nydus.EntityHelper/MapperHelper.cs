using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nydus.EntityHelper.Exceptions;

namespace Nydus.EntityHelper;

public class MapperHelper<TEntity> : IMapperHelper<TEntity> where TEntity : class, new()
{
    private readonly IConfigurationProvider _config;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IMapper _mapper;
    private readonly IKey _primaryKey;
    protected readonly DbContext DbContext;
    private IQueryable<TEntity> _queryable;
    
    public virtual IQueryable<TEntity> Entities => _queryable;
    
    public MapperHelper(DbContext dbContext, IMapper mapper, MapperHelperOptions<TEntity>? options = null)
    {
        DbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
        _queryable = _dbSet;
        _mapper = mapper;
        _config = mapper.ConfigurationProvider;
        _primaryKey = dbContext.Model?
            .FindEntityType(typeof(TEntity))?
            .FindPrimaryKey() ?? throw new Exception($"Could not find primary key for Entity {nameof(TEntity)}");
        
        SetOptions(options);
    }

    public void SetOptions(MapperHelperOptions<TEntity>? options = null)
    {
        _queryable = options?.GlobalFilter != null ? _dbSet.Where(options.GlobalFilter) : _dbSet;
    }

    public IQueryable<TDto> Dtos<TDto>()
    {
        return Entities
            .UseAsDataSource(_config)
            .For<TDto>();
    }

    public async Task<TDto> Create<TDto, TCreate>(TCreate model, TEntity? created = null)
    {
        var entity = created ?? new TEntity();

        _mapper.Map(model, entity);

        HandleCreate(entity);

        _dbSet.Attach(entity);

        await DbContext.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }

    public Task<TDto> Create<TDto>(TDto model, TEntity? created = null)
    {
        return Create<TDto, TDto>(model, created);
    }

    public Task<TDto> Update<TDto, TCreate>(TCreate model, object id)
    {
        var entity = Find(id);

        return Update<TDto, TCreate>(model, entity);
    }
    
    public Task<TDto> Update<TDto>(TDto model, object id)
    {
        var entity = Find(id);

        return Update<TDto, TDto>(model, entity);
    }

    public async Task<TDto> Update<TDto, TCreate>(TCreate model, TEntity entity)
    {
        HandleUpdate(entity);

        _mapper.Map(model, entity);

        await DbContext.SaveChangesAsync();

        _dbSet.Attach(entity);

        await DbContext.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }
    
    public Task<TDto> Update<TDto>(TDto model, TEntity entity)
    {
        return Update<TDto, TDto>(model, entity);
    }

    public TDto? Get<TDto>(object id)
    {
        var entity = TryFind(id);

        return entity == null ? default : _mapper.Map<TDto>(entity);
    }

    public async Task Delete(object o)
    {
        var deleted = await FindAsync(o);

        HandleDelete(deleted);

        await DbContext.SaveChangesAsync();
    }

    public TDto ToDto<TDto>(TEntity entity, Action<IMappingOperationOptions> opts)
    {
        return _mapper.Map<TDto>(entity, opts);
    }

    public IQueryable<TDto> ToDto<TDto>(IQueryable<TEntity> entities)
    {
        return _mapper.ProjectTo<TDto>(entities);
    }

    protected virtual void HandleUpdate(TEntity e)
    {
    }

    protected virtual void HandleCreate(TEntity e)
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

    // public IEnumerable<string> FindPrimaryKeyNames(TEntity entity) {
    //     return from p in dbContext.FindPrimaryKeyProperties(entity) 
    //         select p.Name;
    // }

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