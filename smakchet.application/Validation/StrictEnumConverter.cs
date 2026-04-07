namespace smakchet.application.Validation
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class StrictEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Number)
                throw new JsonException($"Expected number for enum {typeof(T).Name}");

            int intValue = reader.GetInt32();
            if (!Enum.IsDefined(typeof(T), intValue))
                throw new JsonException($"Invalid value '{intValue}' for enum {typeof(T).Name}");

            return (T)Enum.ToObject(typeof(T), intValue);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(Convert.ToInt32(value));
        }
    }
}