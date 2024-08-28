using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartBuildingVisualization.Client.Services;
using Blazored.LocalStorage;
using SmartBuildingVisualization.Client.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using SmartBuildingVisualization.Client.Core.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddScoped<IDataStorage, DataStorageService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<CacheService>();
builder.Services.AddSingleton<NotificationsService>();
builder.Services.AddMemoryCache(options => { options.SizeLimit = 500; });
builder.Services.AddSingleton<IDateService, DateService>();

builder.Services.AddHttpClient<IDataApiClient, ApiService>(configureClient => {
    configureClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
builder.Services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
builder.Services.AddBlazorBootstrap();



var host = builder.Build();

var dataService = host.Services.GetRequiredService<IDataService>();
await host.RunAsync();


