using System.Text.Json.Serialization;
using System.Text.Json;

namespace LiterasWebAPI.Models;

public class ListEnumToStringConverter<T> : JsonConverter<List<T>> where T : struct, Enum
{
    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var result = new List<T>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (Enum.TryParse(reader.GetString(), out T enumValue))
                {
                    result.Add(enumValue);
                }
            }
            return result;
        }
        else
        {
            throw new JsonException("Expected start of array");
        }
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteStringValue(item.ToString());
        }
        writer.WriteEndArray();
    }
}
