using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web_453503.BlazorWasm;
using Web_453503.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
});

var apiUri = builder.Configuration["UriData:ApiUri"]
             ?? throw new InvalidOperationException("API URI is not configured");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUri) });

builder.Services.AddScoped<IDataService,  DataService>();

await builder.Build().RunAsync();