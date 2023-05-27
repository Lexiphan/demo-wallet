using System.Reflection;
using System.Text.Json.Serialization;
using Greentube.DemoWallet.Api;
using Greentube.DemoWallet.Application;
using Greentube.DemoWallet.Database;
using Greentube.DemoWallet.Database.Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Configure configuration ----------------------

// This file is added to .gitignore and .dockerignore files,
// thus every developer will have chance to set his own configuration for any testing purposes
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false);

// --------------------- Configure services -------------------------

builder.Services.AddControllers()
    .AddJsonOptions(
        opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));
builder.Services.AddApiVersioning(options => options.DefaultApiVersion = new ApiVersion(1, 0));
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Major, optional minor version, and suffix: 1-rc -> 1-rc, 1.1 -> 1.1
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(opts => opts.IncludeXmlComments(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "xml"), true));
builder.Services.ConfigureOptions<ConfigureVersioningSwaggerOptions>();

builder.Services
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddWalletDatabase()
    .AddApplicationServices();

// ---------------- Configure application pipeline ------------------

var app = builder.Build();

app.MapControllers();

// Basic endpoints which can be used for health nad deployment checks
var thisAssemblyName = Assembly.GetExecutingAssembly().GetName();
app.MapGet("/", () => thisAssemblyName.Name);
app.MapGet("/version", () => thisAssemblyName.Version?.ToString());

app.UseSwagger();
app.UseSwaggerUI();

app.MapFallback(async (HttpContext httpContext, CancellationToken ct) =>
{
    httpContext.Response.StatusCode = 404;
    await httpContext.Response.WriteAsync("404 Not Found", ct);
});

// ------------------------------ Run -------------------------------

// First ensure all migrations are applied to the DB
await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<IWalletDatabase>().MigrateAsync();
}

app.Run();
