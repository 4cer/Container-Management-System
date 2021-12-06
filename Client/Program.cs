using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using ProITM.Client.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("ProITM.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ProITM.ServerAPI"));

            builder.Services.AddScoped<IAdministrationService, AdministrationService>();

            // Adding the service, such that it is globally available in client Blazor views:
            //     Note that interface and its implementing classes are passed, allowing for
            //     swapping the latter, simplifying the testing
            // builder.Services.AddScoped<IObsoleteExampleService, ObsoleteExampleService>();

            builder.Services.AddScoped<IAdminContainerService, AdminContainerService>();
            builder.Services.AddScoped<IHostService, HostService>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddApiAuthorization();

            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}
