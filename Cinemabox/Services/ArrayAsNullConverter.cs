using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cinemabox.Services;

// Deserializes a JSON object as T, but returns null when the value is a JSON array.
public class ArrayAsNullConverter<T> : JsonConverter<T?> where T : class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Skip the entire array
            using var doc = JsonDocument.ParseValue(ref reader);
            return null;
        }

        return JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, options);
}
