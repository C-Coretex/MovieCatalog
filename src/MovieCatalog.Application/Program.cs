using MovieCatalog.Application.Components;
using MovieCatalog.Application.Extensions;
using MovieCatalog.Infrastructure.Migrations.Factory;

var builder = WebApplication.CreateBuilder(args);


var movieCatalogConnectionString = builder.Configuration.GetConnectionString("MovieCatalogConnectionString")
                                    ?? throw new ArgumentException("'MovieCatalogConnectionString' is not configured.");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.RegisterServices(builder.Configuration, movieCatalogConnectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    MovieCatalogMigrationsDbContext.ApplyMigrations(movieCatalogConnectionString);
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
