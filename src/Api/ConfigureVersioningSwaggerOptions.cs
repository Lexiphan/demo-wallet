using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Greentube.DemoWallet.Api;

/// <summary>
/// The class which registers all IConfigureOptions necessary for versioning in Swagger.
/// </summary>
internal class ConfigureVersioningSwaggerOptions :
    IConfigureOptions<SwaggerGenOptions>,
    IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new() { Version = description.GroupName, Title = "Wallet API" });
        }
    }

    public void Configure(SwaggerUIOptions options)
    {
        var apiVersionDescriptions = _apiVersionDescriptionProvider.ApiVersionDescriptions;

        // Reverse order of version - to show the latest version first
        for (var i = apiVersionDescriptions.Count - 1; i >= 0; i--)
        {
            var description = apiVersionDescriptions[i];
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Wallet API {description.GroupName}");
        }
    }

    public ConfigureVersioningSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }
}
