using System.Collections;
using System.Collections.Generic;

namespace Nydus.Fop.Pagination;

public class PageResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public IList<T> Content { get; set; }
}

public class PageResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public IList Content { get; set; }
}