using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieCatalog.Providers.Omdb.Converters
{
    internal class DateOnlyConverter : JsonConverter<DateOnly?>
    {
        private const string Format = "dd MMM yyyy";

        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.TryParseExact(value, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
