namespace MovieCatalog.Providers.Omdb.Helpers
{
    internal static class StringHelpers
    {
        public static string FormatNullableString(this string? value)
        {
            return value ?? "N/A";
        }

        public static bool IsStringEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value) || string.Equals(value, "N/A", StringComparison.OrdinalIgnoreCase);
        }
    }
}
