using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Nydus.IntegrationTestsServer;
using Xunit;

namespace Nydus.Tests.Integration.ControllerTests;

public abstract class ControllerTestBase
{
    private readonly string _baseUrl;
    private readonly HttpClient _client = IntegrationTestServer.GetTestServer().CreateClient();

    protected ControllerTestBase(string baseUrl = null)
    {
        _baseUrl = baseUrl;
    }

    protected Task<T> GetResponse<T>(string baseUrl, IEnumerable<KeyValuePair<string, string>> query)
    {
        var queryParams = query ?? new Dictionary<string, string>();

        var queryBuilder = new QueryBuilder(queryParams);
        return GetResponse<T>(baseUrl + queryBuilder.ToQueryString());
    }

    protected Task<T> GetResponse<T>(IEnumerable<KeyValuePair<string, string>> query)
    {
        return GetResponse<T>(_baseUrl, query);
    }

    protected Task<T> GetResponse<T>()
    {
        return GetResponse<T>(_baseUrl, null);
    }

    protected async Task<T> GetResponse<T>(string url)
    {
        var response = await _client.GetAsync(url);
        Assert.True(response.IsSuccessStatusCode, "The HttpResponse indicates a non-success status code");
        var responseContent = await response.Content.ReadAsStringAsync();
        var deserializedResponse = JsonConvert.DeserializeObject<T>(responseContent);
        return deserializedResponse;
    }

    protected Task<HttpResponseMessage> GetResponseMessage(string url)
    {
        return _client.GetAsync(url);
    }
}