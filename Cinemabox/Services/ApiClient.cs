using System.Text.Json;

namespace Cinemabox.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _username;
    private readonly string _password;

    public ApiClient(HttpClient httpClient, SettingsService settingsService)
    {
        var settings = settingsService.Settings;
        _username = settings?.Username ?? string.Empty;
        _password = settings?.Password ?? string.Empty;
        if (settings is not null)
            httpClient.BaseAddress = new Uri(settings.BaseUrl);
        _httpClient = httpClient;
    }

    public async Task<AccountInfoResponse> GetAccountInfoAsync()
    {
        var url = $"/player_api.php?username={Uri.EscapeDataString(_username)}&password={Uri.EscapeDataString(_password)}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var wrapper = JsonSerializer.Deserialize<AccountInfoApiResponse>(json)
            ?? throw new InvalidOperationException("Failed to deserialize account info response.");
        return wrapper.UserInfo;
    }

    public async Task<List<VodCategoryResponse>> GetVodCategoriesAsync()
    {
        var url = $"/player_api.php?username={Uri.EscapeDataString(_username)}&password={Uri.EscapeDataString(_password)}&action=get_vod_categories";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<VodCategoryResponse>>(json)
            ?? throw new InvalidOperationException("Failed to deserialize VOD categories response.");
    }

    public async Task<List<VodCategoryResponse>> GetSeriesCategoriesAsync()
    {
        var url = $"/player_api.php?username={Uri.EscapeDataString(_username)}&password={Uri.EscapeDataString(_password)}&action=get_series_categories";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<VodCategoryResponse>>(json)
            ?? throw new InvalidOperationException("Failed to deserialize series categories response.");
    }

    public async Task<List<VodStreamResponse>> GetVodStreamsAsync(string categoryId)
    {
        var url = $"/player_api.php?username={Uri.EscapeDataString(_username)}&password={Uri.EscapeDataString(_password)}&action=get_vod_streams&category_id={Uri.EscapeDataString(categoryId)}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<VodStreamResponse>>(json)
            ?? throw new InvalidOperationException("Failed to deserialize VOD streams response.");
    }

    public async Task<List<SeriesResponse>> GetSeriesAsync(string categoryId)
    {
        var url = $"/player_api.php?username={Uri.EscapeDataString(_username)}&password={Uri.EscapeDataString(_password)}&action=get_series&category_id={Uri.EscapeDataString(categoryId)}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<SeriesResponse>>(json)
            ?? throw new InvalidOperationException("Failed to deserialize series response.");
    }

    public async Task<SeriesInfoResponse> GetSeriesInfoAsync(int seriesId)
    {
        var url = $"/player_api.php?username={Uri.EscapeDataString(_username)}&password={Uri.EscapeDataString(_password)}&action=get_series_info&series_id={seriesId}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<SeriesInfoResponse>(json)
            ?? throw new InvalidOperationException("Failed to deserialize series info response.");
    }

    public async Task<VodInfoResponse> GetVodInfoAsync(int vodId)
    {
        var url = $"/player_api.php?username={Uri.EscapeDataString(_username)}&password={Uri.EscapeDataString(_password)}&action=get_vod_info&vod_id={vodId}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<VodInfoResponse>(json)
            ?? throw new InvalidOperationException("Failed to deserialize VOD info response.");
    }
}
