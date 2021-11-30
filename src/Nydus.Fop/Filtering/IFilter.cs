using System.Linq;

namespace Nydus.Fop.Filtering;

public interface IFilter<T> : IFilter
{
    IQueryable IFilter.Apply(IQueryable queryable)
    {
        return Apply(queryable as IQueryable<T>);
    }

    public IQueryable<T> Apply(IQueryable<T> queryable);
}

public interface IFilter
{
    public IQueryable Apply(IQueryable queryable);
}