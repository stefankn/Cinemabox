using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cinemabox.Services;

// Some API responses return numeric fields as either a number or a quoted string.
public class FlexibleDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String => double.TryParse(reader.GetString(), NumberStyles.Any,
                CultureInfo.InvariantCulture, out var d) ? d : 0,
            _ => 0
        };

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value);
}
