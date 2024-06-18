using als_tools.ui.web.Extensions;
using als_tools.ui.web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


try
{
    var embeddedDbContext = app.Services.GetRequiredService<IEmbeddedDatabaseContext>();
    embeddedDbContext.Initialize();

    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
