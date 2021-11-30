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

    public MapperHelper(DbContext dbContext, IMapper mapper, MapperHelperOptions<TEntity>? options)
    {
        DbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
        Entities = options?.GlobalFilter != null ? _dbSet.Where(options.GlobalFilter) : _dbSet;
        _mapper = mapper;
        _config = mapper.ConfigurationProvider;
        _primaryKey = dbContext.Model
            .FindEntityType(typeof(TEntity))
            .FindPrimaryKey();
    }

    public MapperHelper(DbContext dbContext, IMapper mapper) : this(dbContext, mapper, null)
    {
    }

    public IQueryable<TDto> Dtos<TDto>()
    {
        return Entities
            .UseAsDataSource(_config)
            .For<TDto>();
    }

    public virtual IQueryable<TEntity> Entities { get; }

    public async Task<TDto> Create<TDto, TCreate>(TCreate model, TEntity? created = null)
    {
        var entity = created ?? new TEntity();

        _mapper.Map(model, entity);

        HandleCreate(entity);

        _dbSet.Attach(entity);

        await DbContext.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }

    public Task<TDto> Update<TDto, TCreate>(TCreate model, object id)
    {
        var entity = Find(id);

        return Update<TDto, TCreate>(model, entity);
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

    public virtual TEntity? TryFind(object id)
    {
        return _dbSet.Find(id);
    }

    public TDto? Get<TDto>(object id)
    {
        var entity = TryFind(id);

        return entity == null ? default : _mapper.Map<TDto>(entity);
    }

    public async Task Delete(object o)
    {
        var deleted = await _dbSet.FindAsync(o);

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