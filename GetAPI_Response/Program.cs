using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GetAPI_Response;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "UpdateDBService";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient<CheckAndFillDBService>();
    })
    .Build();

await host.RunAsync();