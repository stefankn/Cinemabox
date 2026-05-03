using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

namespace Cinemabox.Services;

public class SettingsService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly IDataProtector _protector;
    private readonly string _filePath;
    private AppSettings? _settings;

    public SettingsService(IDataProtectionProvider dataProtection)
    {
        _protector = dataProtection.CreateProtector("Cinemabox.Settings.v1");

        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Cinemabox");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "settings.json");

        Load();
    }

    public bool IsConfigured => _settings is not null;
    public AppSettings? Settings => _settings;

    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var json = File.ReadAllText(_filePath);
            var raw = JsonSerializer.Deserialize<RawSettings>(json);
            if (raw is null) return;

            _settings = new AppSettings
            {
                BaseUrl = raw.BaseUrl,
                Username = _protector.Unprotect(raw.Username),
                Password = _protector.Unprotect(raw.Password),
            };
        }
        catch
        {
            // Corrupted or unreadable settings — treat as unconfigured
            _settings = null;
        }
    }

    public void Reset()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
        _settings = null;
    }

    public void Save(string baseUrl, string username, string password)
    {
        var raw = new RawSettings
        {
            BaseUrl = baseUrl,
            Username = _protector.Protect(username),
            Password = _protector.Protect(password),
        };

        var json = JsonSerializer.Serialize(raw, JsonOptions);
        File.WriteAllText(_filePath, json);

        _settings = new AppSettings { BaseUrl = baseUrl, Username = username, Password = password };
    }

    private sealed class RawSettings
    {
        public string BaseUrl { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
