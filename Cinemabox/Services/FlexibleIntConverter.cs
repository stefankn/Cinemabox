using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cinemabox.Services;

// Some API responses return integer fields as either a number or a quoted string.
public class FlexibleIntConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.Number => reader.TryGetInt32(out var i) ? i : null,
            JsonTokenType.String => int.TryParse(reader.GetString(), out var i) ? i : null,
            JsonTokenType.Null => null,
            _ => null
        };

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value is null) writer.WriteNullValue();
        else writer.WriteNumberValue(value.Value);
    }
}
