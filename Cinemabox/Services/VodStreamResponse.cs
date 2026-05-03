using System.Text.Json.Serialization;

namespace Cinemabox.Services;

public class VodStreamResponse
{
    [JsonPropertyName("num")]
    public int Num { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("stream_id")]
    public int StreamId { get; set; }

    [JsonPropertyName("stream_type")]
    public string StreamType { get; set; } = "movie";

    [JsonPropertyName("stream_icon")]
    public string? StreamIcon { get; set; }

    [JsonPropertyName("rating")]
    public string? Rating { get; set; }

    [JsonPropertyName("rating_5based")]
    [JsonConverter(typeof(FlexibleDoubleConverter))]
    public double Rating5Based { get; set; }

    [JsonPropertyName("tmdb")]
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string? Tmdb { get; set; }

    [JsonPropertyName("trailer")]
    public string? Trailer { get; set; }

    [JsonPropertyName("added")]
    public string? Added { get; set; }

    [JsonPropertyName("is_adult")]
    public int IsAdult { get; set; }

    [JsonPropertyName("category_id")]
    public string? CategoryId { get; set; }

    [JsonPropertyName("container_extension")]
    public string? ContainerExtension { get; set; }
}
