using MovieCatalog.Providers.Omdb.Helpers;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.Converters
{
    internal class RatingFloatConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value.IsStringEmpty())
                return 0;

            var isPercent = value!.EndsWith("%", StringComparison.Ordinal);
            if(isPercent)
                return float.Parse(value.TrimEnd('%'), CultureInfo.InvariantCulture) / 100;

            var splittedElements = value.Split('/');
            if (splittedElements.Length != 2)
                throw new ArgumentException($"Unexpected rating format. Value: '{value}'", nameof(value));

            return float.Parse(splittedElements[0], CultureInfo.InvariantCulture) / float.Parse(splittedElements[1], CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
