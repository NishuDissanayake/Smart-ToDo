using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToDoApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ToDoService>();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
