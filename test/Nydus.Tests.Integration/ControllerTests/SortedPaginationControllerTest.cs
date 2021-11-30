using System.Collections.Generic;
using System.Threading.Tasks;
using Nydus.Fop.Pagination;
using Nydus.IntegrationTestsServer;
using Xunit;

namespace Nydus.Tests.Integration.ControllerTests;

public class SortedPaginationControllerTest : ControllerTestBase
{
    public readonly int DefaultPage = 1;
    public readonly int DefaultPageSize = 4;
    public readonly int ItemsCount = 10;

    public SortedPaginationControllerTest() : base("SortedPaginated")
    {
    }

    [Fact]
    public async Task ReturnsPaginationResultOnRequestingWithoutParams()
    {
        var response = await GetResponse<PageResult<User>>();
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ReturnsPaginationResultOnRequestingWithPageSize()
    {
        var pageSize = 2;
        var response = await GetResponse<PageResult<User>>(
            new[]
            {
                new KeyValuePair<string, string>("PageSize", $"{pageSize}"),
            });
        Assert.Equal(DefaultPage, response.Page);
        Assert.Equal(ItemsCount, response.TotalCount);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(pageSize, response.Content.Count);
    }

    [Fact]
    public async Task ReturnsPaginationResultOnRequestingWithPage()
    {
        var page = 2;
        var response = await GetResponse<PageResult<User>>(
            new[]
            {
                new KeyValuePair<string, string>("Page", $"{page}"),
            });
        Assert.Equal(page, response.Page);
        Assert.Equal(ItemsCount, response.TotalCount);
        Assert.Equal(DefaultPageSize, response.PageSize);
        Assert.Equal(DefaultPageSize, response.Content.Count);
    }

    [Fact]
    public async Task ReturnsPaginationResultOnRequestingPageAndPage()
    {
        var pageSize = 2;
        var page = 2;
        var response = await GetResponse<PageResult<User>>(
            new[]
            {
                new KeyValuePair<string, string>("PageSize", $"{pageSize}"),
                new KeyValuePair<string, string>("Page", $"{page}"),
            });
        Assert.Equal(page, response.Page);
        Assert.Equal(ItemsCount, response.TotalCount);
        Assert.Equal(pageSize, response.PageSize);
        Assert.Equal(pageSize, response.Content.Count);
    }

    [Fact]
    public async Task ReturnsPaginationResultOnRequestingWithSort()
    {
        var response = await GetResponse<PageResult<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+userName"),
            });
        Assert.Equal(DefaultPage, response.Page);
        Assert.Equal(ItemsCount, response.TotalCount);
        Assert.Equal(DefaultPageSize, response.PageSize);
        Assert.Equal(DefaultPageSize, response.Content.Count);
        var content = response.Content;
        for (var i = 1; i < content.Count; i++)
        {
            var previousValueIsSmaller = string.CompareOrdinal(content[i].UserName, content[i - 1].UserName) > 0;
            Assert.True(previousValueIsSmaller);
        }
    }
}