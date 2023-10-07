using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace OptionPatternsWithValidation.UnitTests;

public class OptionsTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly string configKey = "ApiKey";

    public OptionsTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/options")]
    [InlineData("/optionsSnapshot")]
    [InlineData("/optionsMonitor")]
    public async Task Options_ReturnsCorrectConfig(string endpoint)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(endpoint);

        // Assert
        response.EnsureSuccessStatusCode();
        var respOptions = await response.Content.ReadFromJsonAsync<AppOptions>();
        Assert.NotNull(respOptions);
        Assert.Equal(configKey, respOptions.ApiKey);
    }
}

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public IConfiguration Configuration { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            var config = configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"AppOptions:ApiKey", "ApiKey" }
                });
            Configuration = config.Build();
        });
    }
}