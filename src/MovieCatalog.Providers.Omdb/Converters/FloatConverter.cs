using MovieCatalog.Providers.Omdb.Helpers;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.Converters
{
    internal class FloatConverter : JsonConverter<float?>
    {
        public override float? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value.IsStringEmpty())
                return null;

            return float.Parse(value!, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, float? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(CultureInfo.InvariantCulture));
        }
    }
}
