using System.Collections.Concurrent;

namespace Cinemabox.Services;

public class VodCoverService
{
    private readonly string _coversDir;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<VodCoverService> _logger;
    private readonly SemaphoreSlim _downloadSemaphore = new(8, 8);
    private readonly ConcurrentDictionary<string, bool> _failed = new();

    public VodCoverService(IHttpClientFactory httpClientFactory, ILogger<VodCoverService> logger)
    {
        _coversDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Cinemabox", "vod-covers");
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        Directory.CreateDirectory(_coversDir);
    }

    public string GetCoverUrl(string categoryId, int streamId, string? remoteUrl)
    {
        var key = $"{categoryId}-{streamId}";
        if (_failed.ContainsKey(key)) return string.Empty;

        var fileName = GetFileName(categoryId, streamId, remoteUrl);
        if (fileName is not null && File.Exists(Path.Combine(_coversDir, fileName)))
            return $"/vod-covers/{fileName}";

        return remoteUrl ?? string.Empty;
    }

    public async Task CacheCoverAsync(string categoryId, int streamId, string? remoteUrl)
    {
        if (string.IsNullOrEmpty(remoteUrl)) return;

        var key = $"{categoryId}-{streamId}";
        if (_failed.ContainsKey(key)) return;

        var fileName = GetFileName(categoryId, streamId, remoteUrl);
        if (fileName is null) return;

        var localPath = Path.Combine(_coversDir, fileName);
        if (File.Exists(localPath)) return;

        await _downloadSemaphore.WaitAsync();
        try
        {
            if (File.Exists(localPath)) return;

            var http = _httpClientFactory.CreateClient("vod-covers");
            var bytes = await http.GetByteArrayAsync(remoteUrl);
            await File.WriteAllBytesAsync(localPath, bytes);
        }
        catch (Exception)
        {
            _failed.TryAdd(key, true);
        }
        finally
        {
            _downloadSemaphore.Release();
        }
    }

    public async Task CacheAllAsync(string categoryId, IEnumerable<VodStreamResponse> streams)
    {
        var tasks = streams
            .Where(s => !string.IsNullOrEmpty(s.StreamIcon))
            .Select(s => CacheCoverAsync(categoryId, s.StreamId, s.StreamIcon));

        await Task.WhenAll(tasks);
    }

    private static string? GetFileName(string categoryId, int streamId, string? remoteUrl)
    {
        if (string.IsNullOrEmpty(remoteUrl)) return null;
        var ext = Path.GetExtension(new Uri(remoteUrl).AbsolutePath);
        if (string.IsNullOrEmpty(ext)) ext = ".jpg";
        return $"{categoryId}-{streamId}{ext}";
    }
}
