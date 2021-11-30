using System.Collections.Generic;
using System.Threading.Tasks;
using Nydus.Fop.Pagination;
using Nydus.IntegrationTestsServer;
using Xunit;

namespace Nydus.Tests.Integration.ControllerTests;

public class PaginatedControllerTest : ControllerTestBase
{
    public PaginatedControllerTest() : base("Paginated")
    {
    }

    [Fact]
    public async Task PaginatesWithoutQuery()
    {
        var pagedResult = await GetResponse<PageResult<User>>();
        Assert.Equal(10, pagedResult.TotalCount);
        Assert.Equal(1, pagedResult.Page);
        Assert.Equal(4, pagedResult.PageSize);
        Assert.Equal(4, pagedResult.Content.Count);
    }

    [Fact]
    public async Task FirstPage()
    {
        var pagedResult = await GetResponse<PageResult<User>>(
            new[]
            {
                new KeyValuePair<string, string>("page", "1"),
            });
        Assert.Equal(10, pagedResult.TotalCount);
        Assert.Equal(1, pagedResult.Page);
        Assert.Equal(4, pagedResult.PageSize);
        Assert.Equal(4, pagedResult.Content.Count);
        Assert.Contains(pagedResult.Content, u => u.UserName == "Alice");
        Assert.Contains(pagedResult.Content, u => u.UserName == "Bob");
        Assert.Contains(pagedResult.Content, u => u.UserName == "Caroline");
        Assert.Contains(pagedResult.Content, u => u.UserName == "Dave");
    }

    [Fact]
    public async Task SecondPage()
    {
        var pagedResult = await GetResponse<PageResult<User>>(
            new[]
            {
                new KeyValuePair<string, string>("page", "2"),
            });
        Assert.Equal(10, pagedResult.TotalCount);
        Assert.Equal(2, pagedResult.Page);
        Assert.Equal(4, pagedResult.PageSize);
        Assert.Equal(4, pagedResult.Content.Count);

        Assert.Contains(pagedResult.Content, u => u.UserName == "Emilia");
        Assert.Contains(pagedResult.Content, u => u.UserName == "Fred");
        Assert.Contains(pagedResult.Content, u => u.UserName == "Georgia");
        Assert.Contains(pagedResult.Content, u => u.UserName == "Hank");
    }


    [Fact]
    public async Task LastPage()
    {
        var pagedResult = await GetResponse<PageResult<User>>(
            new[]
            {
                new KeyValuePair<string, string>("page", "3"),
            });
        Assert.Equal(10, pagedResult.TotalCount);
        Assert.Equal(3, pagedResult.Page);
        Assert.Equal(4, pagedResult.PageSize);
        Assert.Equal(2, pagedResult.Content.Count);
        Assert.Contains(pagedResult.Content, u => u.UserName == "Iris");
        Assert.Contains(pagedResult.Content, u => u.UserName == "Joe");
    }

    // TODO What do with un-existing pages?
    // [Fact]
    // public async Task GetUnexistingPage()
    // {
    //     var pagedResult = await GetResponse<PageResult<User>>( new []
    //         {
    //             new KeyValuePair<string, string>("page", "5")
    //         }
    //     );
    //     Assert.Equal(10, pagedResult.TotalCount);
    //     Assert.Equal(4, pagedResult.Page);
    //     Assert.Equal(3, pagedResult.PageSize);
    //     Assert.Single(pagedResult.Content);
    //     Assert.Contains(pagedResult.Content, u => u.UserName == "Joe");
    // }
}