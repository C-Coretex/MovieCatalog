using MovieCatalog.Providers.Omdb.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.Converters
{
    internal class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return !value.IsStringEmpty() && value!.Equals("True", StringComparison.Ordinal);
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            => writer.WriteStringValue(value ? "True" : "False");
    }
}
