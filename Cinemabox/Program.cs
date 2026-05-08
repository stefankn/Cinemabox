using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.DataProtection;
using Cinemabox.Services;
using App = Cinemabox.Components.App;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var keysDir = new DirectoryInfo(Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "Cinemabox", "keys"));
keysDir.Create();
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(keysDir)
    .SetApplicationName("Cinemabox");
builder.Services.AddSingleton<SettingsService>();
builder.Services.AddSingleton<CatalogCache>();

builder.Services.AddHttpClient<ApiClient>();
builder.Services.AddSingleton<VodCoverService>();
builder.Services.AddSingleton<DownloadService>();
builder.Services.AddHttpClient("vod-covers");

builder.Services.AddElectron();
builder.WebHost.UseElectron(args, async () =>
{
    var options = new BrowserWindowOptions
    {
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

var coverDir = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "Cinemabox", "vod-covers");
Directory.CreateDirectory(coverDir);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(coverDir),
    RequestPath = "/vod-covers"
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.UseStaticFiles();
app.MapStaticAssets();
app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
