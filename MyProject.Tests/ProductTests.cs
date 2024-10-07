using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ProductTests : IClassFixture<WebApplicationFactory<EcommerceApi.Program>>
{
    private readonly HttpClient _client;

    public ProductTests(WebApplicationFactory<EcommerceApi.Program> factory)
    {
        this._client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProductUsuarioSemToken_DeveRetornarUnauthorized()
    {
        // Arrange: Define a URL para o endpoint da API
        var url = "http://localhost:5251/api/product";

        // Act: Faz a requisição GET para a API
        var response = await _client.GetAsync(url);

        // Assert: Verifica se o status code da resposta foi Unauthorized (401)
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
