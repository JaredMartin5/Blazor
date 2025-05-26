using Blazored.LocalStorage;
using Client;
using Client.Helpers;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

builder.Services.Scan(scan => scan.FromAssembliesOf(typeof(AccountsService)).AddClasses(classes => classes.Where(x => x.Name.EndsWith("Service"))).AsImplementedInterfaces().WithScopedLifetime());

builder.Services.AddScoped<FormSubmissionHelper>();

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7109/api"), });

await builder.Build().RunAsync();
