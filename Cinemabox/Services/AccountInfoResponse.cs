using System.Text.Json.Serialization;

namespace Cinemabox.Services;

public class AccountInfoResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("exp_date")]
    public string ExpDate { get; set; } = string.Empty;
}

public class AccountInfoApiResponse
{
    [JsonPropertyName("user_info")]
    public AccountInfoResponse UserInfo { get; set; } = new();
}
