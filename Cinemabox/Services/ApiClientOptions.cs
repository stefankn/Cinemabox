namespace Cinemabox.Services;

public class ApiClientOptions
{
    public const string SectionName = "ApiClient";
    public string BaseUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string BuildStreamUrl(string streamType, int streamId, string extension)
    {
        if (string.IsNullOrEmpty(BaseUrl) || string.IsNullOrEmpty(Username) ||
            string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(streamType) ||
            string.IsNullOrEmpty(extension))
            return string.Empty;

        return $"{BaseUrl.TrimEnd('/')}/{streamType}/{Uri.EscapeDataString(Username)}/{Uri.EscapeDataString(Password)}/{streamId}.{extension}";
    }

    public string BuildSeriesEpisodeUrl(string episodeId, string containerExtension)
    {
        if (string.IsNullOrEmpty(BaseUrl) || string.IsNullOrEmpty(Username) ||
            string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(episodeId) ||
            string.IsNullOrEmpty(containerExtension))
            return string.Empty;

        return $"{BaseUrl.TrimEnd('/')}/series/{Uri.EscapeDataString(Username)}/{Uri.EscapeDataString(Password)}/{episodeId}.{containerExtension}";
    }
}
