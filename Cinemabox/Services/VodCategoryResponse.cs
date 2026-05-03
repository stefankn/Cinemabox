using System.Text.Json.Serialization;

namespace Cinemabox.Services;

public class VodCategoryResponse
{
    [JsonPropertyName("category_id")]
    public string CategoryId { get; set; } = string.Empty;

    [JsonPropertyName("category_name")]
    public string CategoryName { get; set; } = string.Empty;
}
