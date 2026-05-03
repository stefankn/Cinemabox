using ElectronNET.API;
using ElectronNET.API.Entities;
using App = Cinemabox.Components.App;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddElectron();
builder.WebHost.UseElectron(args, async () => {
    var options = new BrowserWindowOptions {
        Width = 1280,
        Height = 800,
        Show = false,
    };
    if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
        options.AutoHideMenuBar = true;
    
    var window = await Electron.WindowManager.CreateWindowAsync(options);
    window.OnReadyToShow += window.Show;
    window.OnClosed += () => Electron.App.Quit();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.MapStaticAssets();
app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();