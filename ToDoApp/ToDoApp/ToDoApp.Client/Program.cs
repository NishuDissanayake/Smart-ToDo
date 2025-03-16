using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToDoApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<ToDoService>();

await builder.Build().RunAsync();
