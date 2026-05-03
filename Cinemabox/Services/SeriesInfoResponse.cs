using System.Text.Json.Serialization;

namespace Cinemabox.Services;

public class SeriesInfoResponse
{
    [JsonPropertyName("seasons")]
    public List<SeriesSeason>? Seasons { get; set; }

    [JsonPropertyName("info")]
    public SeriesInfoDetail? Info { get; set; }

    [JsonPropertyName("episodes")]
    public Dictionary<string, List<SeriesEpisode>>? Episodes { get; set; }
}

public class SeriesSeason
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("season_number")]
    public int? SeasonNumber { get; set; }

    [JsonPropertyName("episode_count")]
    [JsonConverter(typeof(FlexibleIntConverter))]
    public int? EpisodeCount { get; set; }

    [JsonPropertyName("air_date")]
    public string? AirDate { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("cover")]
    public string? Cover { get; set; }

    [JsonPropertyName("cover_big")]
    public string? CoverBig { get; set; }
}

public class SeriesInfoDetail
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("cover")]
    public string? Cover { get; set; }

    [JsonPropertyName("plot")]
    public string? Plot { get; set; }

    [JsonPropertyName("cast")]
    public string? Cast { get; set; }

    [JsonPropertyName("director")]
    public string? Director { get; set; }

    [JsonPropertyName("genre")]
    public string? Genre { get; set; }

    [JsonPropertyName("releaseDate")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("rating")]
    public string? Rating { get; set; }

    [JsonPropertyName("rating_5based")]
    [JsonConverter(typeof(FlexibleDoubleConverter))]
    public double Rating5Based { get; set; }

    [JsonPropertyName("tmdb")]
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string? Tmdb { get; set; }

    [JsonPropertyName("youtube_trailer")]
    public string? YoutubeTrailer { get; set; }

    [JsonPropertyName("episode_run_time")]
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string? EpisodeRunTime { get; set; }

    [JsonPropertyName("category_id")]
    public string? CategoryId { get; set; }
}

public class SeriesEpisode
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string? Id { get; set; }

    [JsonPropertyName("episode_num")]
    public int? EpisodeNum { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("container_extension")]
    public string? ContainerExtension { get; set; }

    [JsonPropertyName("season")]
    public int? Season { get; set; }

    [JsonPropertyName("added")]
    public string? Added { get; set; }

    [JsonPropertyName("custom_sid")]
    public string? CustomSid { get; set; }

    [JsonPropertyName("direct_source")]
    public string? DirectSource { get; set; }
}
