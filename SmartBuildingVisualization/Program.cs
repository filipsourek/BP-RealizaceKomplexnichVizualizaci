using Blazored.LocalStorage;
using DevExpress.Blazor;
using Microsoft.AspNetCore.ResponseCompression;
using SmartBuildingVisualization.Client.Core.Interfaces;
using SmartBuildingVisualization.Client.Services;
using SmartBuildingVisualization.Components;
using SmartBuildingVisualization.Hubs;
using SmartBuildingVisualization.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddScoped<IDataStorage, DataStorageService>();
builder.Services.AddSingleton<StorageService, StorageService>();


builder.Services.AddBlazoredLocalStorage();


#if DEBUG
builder.Services.AddHttpClient<IDataApiClient, ApiService>(configureClient =>
{
    configureClient.BaseAddress = new Uri(builder.Configuration.GetSection("BaseAddress").Value!);
});
#else
builder.Services.AddSingleton(http => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetSection("BaseAddress").Value!)
    // BaseAddress = new Uri("https://smartbuildingvisualization.azurewebsites.net")
});
#endif

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddHttpClient<IDataApiClient, ApiService>(configureClient => {
    configureClient.BaseAddress = new Uri(builder.Configuration.GetSection("BaseAddress").Value!);
});
builder.Services.AddBlazorBootstrap();
builder.Services.AddMvc();
var app = builder.Build();
app.UseResponseCompression();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SmartBuildingVisualization.Client._Imports).Assembly);

app.MapHub<NotifyHub>("/notifyhub");

app.Run();
