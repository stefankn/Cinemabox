using System.Text.Json.Serialization;

namespace Cinemabox.Services;

public class VodInfoResponse
{
    [JsonPropertyName("info")]
    [JsonConverter(typeof(ArrayAsNullConverter<VodInfo>))]
    public VodInfo? Info { get; set; }

    [JsonPropertyName("movie_data")]
    public VodMovieData? MovieData { get; set; }
}

public class VodInfo
{
    [JsonPropertyName("kinopoisk_url")]
    public string? KinopoiskUrl { get; set; }

    [JsonPropertyName("tmdb_id")]
    public string? TmdbId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("o_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("cover_big")]
    public string? CoverBig { get; set; }

    [JsonPropertyName("movie_image")]
    public string? MovieImage { get; set; }

    [JsonPropertyName("releasedate")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("episode_run_time")]
    public int? EpisodeRunTime { get; set; }

    [JsonPropertyName("youtube_trailer")]
    public string? YoutubeTrailer { get; set; }

    [JsonPropertyName("director")]
    public string? Director { get; set; }

    [JsonPropertyName("actors")]
    public string? Actors { get; set; }

    [JsonPropertyName("cast")]
    public string? Cast { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("plot")]
    public string? Plot { get; set; }

    [JsonPropertyName("age")]
    public string? Age { get; set; }

    [JsonPropertyName("mpaa_rating")]
    public string? MpaaRating { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("genre")]
    public string? Genre { get; set; }

    [JsonPropertyName("backdrop_path")]
    public List<string>? BackdropPath { get; set; }

    [JsonPropertyName("duration_secs")]
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string? DurationSecs { get; set; }

    [JsonPropertyName("duration")]
    public string? Duration { get; set; }

    [JsonPropertyName("bitrate")]
    public int? Bitrate { get; set; }

    [JsonPropertyName("rating")]
    public string? Rating { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

public class VodMovieData
{
    [JsonPropertyName("stream_id")]
    public int? StreamId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("added")]
    public string? Added { get; set; }

    [JsonPropertyName("category_id")]
    public string? CategoryId { get; set; }

    [JsonPropertyName("container_extension")]
    public string? ContainerExtension { get; set; }
}
