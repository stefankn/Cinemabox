namespace Cinemabox.Services;

public enum DownloadStatus { Downloading, Completed, Failed, Cancelled }

public class DownloadTask
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; init; } = "";
    public string DestinationPath { get; init; } = "";
    public DownloadStatus Status { get; set; } = DownloadStatus.Downloading;
    public long BytesReceived { get; set; }
    public long? TotalBytes { get; set; }
    public int Progress => TotalBytes is > 0 ? (int)(BytesReceived * 100 / TotalBytes.Value) : -1;
    public CancellationTokenSource Cts { get; } = new();
}

public class DownloadService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly List<DownloadTask> _downloads = [];

    public event Action? OnChanged;

    public IReadOnlyList<DownloadTask> Downloads => _downloads;

    public DownloadService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task StartAsync(string url, string title, string destinationPath)
    {
        var task = new DownloadTask { Title = title, DestinationPath = destinationPath };
        _downloads.Insert(0, task);
        OnChanged?.Invoke();

        try
        {
            var http = _httpClientFactory.CreateClient();
            using var response = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, task.Cts.Token);
            response.EnsureSuccessStatusCode();

            task.TotalBytes = response.Content.Headers.ContentLength;

            await using var src = await response.Content.ReadAsStreamAsync(task.Cts.Token);
            await using var dest = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);

            var buffer = new byte[81920];
            int read;
            while ((read = await src.ReadAsync(buffer, task.Cts.Token)) > 0)
            {
                await dest.WriteAsync(buffer.AsMemory(0, read), task.Cts.Token);
                task.BytesReceived += read;
                OnChanged?.Invoke();
            }

            task.Status = DownloadStatus.Completed;
        }
        catch (OperationCanceledException)
        {
            task.Status = DownloadStatus.Cancelled;
            TryDeletePartial(task.DestinationPath);
        }
        catch
        {
            task.Status = DownloadStatus.Failed;
            TryDeletePartial(task.DestinationPath);
        }

        OnChanged?.Invoke();
    }

    public void Cancel(Guid id)
    {
        _downloads.FirstOrDefault(d => d.Id == id)?.Cts.Cancel();
    }

    public void Dismiss(Guid id)
    {
        var task = _downloads.FirstOrDefault(d => d.Id == id);
        if (task is null || task.Status == DownloadStatus.Downloading) return;
        _downloads.Remove(task);
        OnChanged?.Invoke();
    }

    private static void TryDeletePartial(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); } catch { }
    }
}
