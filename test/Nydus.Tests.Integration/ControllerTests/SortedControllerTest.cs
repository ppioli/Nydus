using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nydus.IntegrationTestsServer;
using Xunit;

namespace Nydus.Tests.Integration.ControllerTests;

public class SortedControllerTest : ControllerTestBase
{
    public SortedControllerTest() : base("Sorted")
    {
    }

    [Fact]
    public async Task WorksWithoutQuery()
    {
        var result = await GetResponse<IEnumerable<User>>();
        Assert.Equal(10, result.Count());
    }

    [Fact]
    public async Task SortsByNestedParameter()
    {
        // var url = "Sorting";
        // var pagedResult = await GetResponse<PaginationResult<User>>(url);
        // Assert.Equal(10, pagedResult.TotalCount);
        // Assert.Equal(4, pagedResult.Page);
        // Assert.Equal(3, pagedResult.PageSize);
        // Assert.Single(pagedResult.Data);
        // Assert.Contains(pagedResult.Data, u => u.UserName == "Joe");
    }


    [Fact]
    public async Task SortById()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+id"),
            });

        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = actualResponse[i].Id > actualResponse[i - 1].Id;
            Assert.True(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByIdWithCamelCase()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+Id"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = actualResponse[i].Id > actualResponse[i - 1].Id;
            Assert.True(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByIdDescending()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "-id"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = actualResponse[i].Id > actualResponse[i - 1].Id;
            Assert.False(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByUserName()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+userName"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = string.Compare(
                                             actualResponse[i].UserName,
                                             actualResponse[i - 1].UserName,
                                             StringComparison.Ordinal) >
                                         0;
            Assert.True(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByUserNameWithCamelCase()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+UserName"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = string.Compare(
                                             actualResponse[i].UserName,
                                             actualResponse[i - 1].UserName,
                                             StringComparison.Ordinal) >
                                         0;
            Assert.True(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByUserNameDescending()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "-userName"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = string.Compare(
                                             actualResponse[i].UserName,
                                             actualResponse[i - 1].UserName,
                                             StringComparison.Ordinal) >
                                         0;
            Assert.False(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByUserNameDescendingWithCamelCase()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "-UserName"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = string.Compare(
                                             actualResponse[i].UserName,
                                             actualResponse[i - 1].UserName,
                                             StringComparison.Ordinal) >
                                         0;
            Assert.False(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByEmail()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+email"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = string.Compare(
                                             actualResponse[i].Email,
                                             actualResponse[i - 1].Email,
                                             StringComparison.Ordinal) >
                                         0;
            Assert.True(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByEmailDescending()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "-email"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller = string.Compare(
                                             actualResponse[i].Email,
                                             actualResponse[i - 1].Email,
                                             StringComparison.Ordinal) >
                                         0;
            Assert.False(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByRegistrationDate()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+registrationDate"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller =
                actualResponse[i].RegistrationDate.CompareTo(actualResponse[i - 1].RegistrationDate) > 0;
            Assert.True(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByRegistrationDateDescending()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "-registrationDate"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller =
                actualResponse[i].RegistrationDate.CompareTo(actualResponse[i - 1].RegistrationDate) > 0;
            Assert.False(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task SortByLastLoginDate()
    {
        // TODO check this. I dont understand this test.
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+registrationDate"),
            });
        Assert.Single(actualResponse.Where(u => u.LastLoginDate != null));
    }

    [Fact]
    public async Task SortByLastLoginDateDescending()
    {
        // TODO check this. I dont understand this test.
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "-registrationDate"),
            });
        Assert.Single(actualResponse.Where(u => u.LastLoginDate != null));
    }


    // TODO Check how does ef resolves this
    // [Fact]
    // public async Task DontSortWithoutSortParameter()
    // {
    //     var url = "Nydus.Fop.Annotations";
    //     var response1 = await GetResponse<List<User>>(url);
    //     var response2 = await GetResponse<List<User>>(url);
    //     var response3 = await GetResponse<List<User>>(url);
    //     var response4 = await GetResponse<List<User>>(url);
    //     Func<IEnumerable<User>, string> aggregateEmails = users =>
    //         users
    //             .Select(u => u.Email)
    //             .Aggregate((current, next) => current + next);
    //     var firstAggregate = aggregateEmails(response1) + aggregateEmails(response2);
    //     var secondAggregate = aggregateEmails(response3) + aggregateEmails(response4);
    //     Assert.NotEqual(firstAggregate, secondAggregate);
    // }

    // TODO Think about this, I think I want to return an error instead 
    // [Fact]
    // public async Task DontSortWithInvalidSortParameter()
    // {
    //     var url = "Nydus.Fop.Annotations?sort=firstName";
    //     var response1 = await GetResponse<List<User>>(url);
    //     var response2 = await GetResponse<List<User>>(url);
    //     var response3 = await GetResponse<List<User>>(url);
    //     var response4 = await GetResponse<List<User>>(url);
    //     Func<IEnumerable<User>, string> aggregateEmails = users =>
    //         users
    //             .Select(u => u.Email)
    //             .Aggregate((current, next) => current + next);
    //     var firstAggregate = aggregateEmails(response1) + aggregateEmails(response2);
    //     var secondAggregate = aggregateEmails(response3) + aggregateEmails(response4);
    //     Assert.NotEqual(firstAggregate, secondAggregate);
    // }

    // TODO Check if ef does this
    // [Fact]
    // public async Task AppliesDefaultSortWithoutClientSortParameter()
    // {
    //     var url = "LightQueryWithDefaultSort";
    //     var actualResponse = await GetResponse<List<User>>(url);
    //     for (var i = 1; i < actualResponse.Count; i++)
    //     {
    //         var previousValueIsSmaller = actualResponse[i].Email.CompareTo(actualResponse[i - 1].Email) > 0;
    //         Assert.True(previousValueIsSmaller);
    //     }
    // }

    // TODO Check if ef does this
    // [Fact]
    // public async Task CanOverrideDefaultSort()
    // {
    //     var url = "LightQueryWithDefaultSort?sort=userName";
    //     var actualResponse = await GetResponse<List<User>>(url);
    //     for (var i = 1; i < actualResponse.Count; i++)
    //     {
    //         var previousValueIsSmaller = actualResponse[i].UserName.CompareTo(actualResponse[i - 1].UserName) > 0;
    //         Assert.True(previousValueIsSmaller);
    //     }
    // }

    [Fact]
    public async Task CanSortByNestedProperty()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+favoriteAnimal.name"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller =
                string.Compare(
                    actualResponse[i].FavoriteAnimal.Name,
                    actualResponse[i - 1].FavoriteAnimal.Name,
                    StringComparison.Ordinal) >
                0;
            Assert.True(previousValueIsSmaller);
        }
    }

    [Fact]
    public async Task CanSortByNestedPropertyDescending()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "-favoriteAnimal.name"),
            });
        for (var i = 1; i < actualResponse.Count; i++)
        {
            var previousValueIsSmaller =
                string.Compare(
                    actualResponse[i].FavoriteAnimal.Name,
                    actualResponse[i - 1].FavoriteAnimal.Name,
                    StringComparison.Ordinal) >
                0;
            Assert.False(previousValueIsSmaller);
        }
    }
    // TODO maybe this should throw an error
    // [Fact]
    // public async Task DoesNotReturnErrorButUnsortedListInCaseOfInvalidPropertyName()
    // {
    //     
    //     var data = Uri.EscapeDataString("+unknownProperty");
    //     var url = "Sorting?sort="+data;
    //     var actualResponse = await GetResponse<List<User>>(url);
    //     Assert.NotEmpty(actualResponse);
    // }

    // TODO maybe this should throw an error
    // [Fact]
    // public async Task DoesNotReturnErrorButUnsortedListInCaseOfInvalidPropertyNameWithRelationalSorting()
    // {
    //     //TODO make this throw an error
    //     var data = Uri.EscapeDataString("+unknownProperty.name");
    //     var url = "Sorting?sort="+data;
    //     var actualResponse = await GetResponse<List<User>>(url);
    //     Assert.NotEmpty(actualResponse);
    // }

    // TODO maybe this should throw an error
    // [Fact]
    // public async Task DoesNotReturnErrorButUnsortedListInCaseOfInvalidPropertyNameWithRelationalSortingWithErrorOnSecondLevel()
    // {
    //     //TODO make this throw an error
    //     var data = Uri.EscapeDataString("+favoriteAnimal.unknownProperty.name");
    //     var url = "Sorting?sort="+data;
    //     var actualResponse = await GetResponse<List<User>>(url);
    //     Assert.NotEmpty(actualResponse);
    // }

    [Fact]
    public async Task SortByGenderThenByUserName()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+gender+userName"),
            });

        Assert.Equal(10, actualResponse.Count);
        Assert.Equal("F", actualResponse[0].Gender);
        Assert.Equal("F", actualResponse[1].Gender);
        Assert.Equal("F", actualResponse[2].Gender);
        Assert.Equal("F", actualResponse[3].Gender);
        Assert.Equal("F", actualResponse[4].Gender);
        Assert.Equal("M", actualResponse[5].Gender);
        Assert.Equal("M", actualResponse[6].Gender);
        Assert.Equal("M", actualResponse[7].Gender);
        Assert.Equal("M", actualResponse[8].Gender);
        Assert.Equal("M", actualResponse[9].Gender);

        Assert.Equal("Alice", actualResponse[0].UserName);
        Assert.Equal("Caroline", actualResponse[1].UserName);
        Assert.Equal("Emilia", actualResponse[2].UserName);
        Assert.Equal("Georgia", actualResponse[3].UserName);
        Assert.Equal("Iris", actualResponse[4].UserName);
        Assert.Equal("Bob", actualResponse[5].UserName);
        Assert.Equal("Dave", actualResponse[6].UserName);
        Assert.Equal("Fred", actualResponse[7].UserName);
        Assert.Equal("Hank", actualResponse[8].UserName);
        Assert.Equal("Joe", actualResponse[9].UserName);
    }

    [Fact]
    public async Task SortByGenderThenByUserNameDescending()
    {
        var actualResponse = await GetResponse<List<User>>(
            new[]
            {
                new KeyValuePair<string, string>("sort", "+gender-userName"),
            });

        Assert.Equal(10, actualResponse.Count);
        Assert.Equal("F", actualResponse[0].Gender);
        Assert.Equal("F", actualResponse[1].Gender);
        Assert.Equal("F", actualResponse[2].Gender);
        Assert.Equal("F", actualResponse[3].Gender);
        Assert.Equal("F", actualResponse[4].Gender);
        Assert.Equal("M", actualResponse[5].Gender);
        Assert.Equal("M", actualResponse[6].Gender);
        Assert.Equal("M", actualResponse[7].Gender);
        Assert.Equal("M", actualResponse[8].Gender);
        Assert.Equal("M", actualResponse[9].Gender);

        Assert.Equal("Iris", actualResponse[0].UserName);
        Assert.Equal("Georgia", actualResponse[1].UserName);
        Assert.Equal("Emilia", actualResponse[2].UserName);
        Assert.Equal("Caroline", actualResponse[3].UserName);
        Assert.Equal("Alice", actualResponse[4].UserName);
        Assert.Equal("Joe", actualResponse[5].UserName);
        Assert.Equal("Hank", actualResponse[6].UserName);
        Assert.Equal("Fred", actualResponse[7].UserName);
        Assert.Equal("Dave", actualResponse[8].UserName);
        Assert.Equal("Bob", actualResponse[9].UserName);
    }
}