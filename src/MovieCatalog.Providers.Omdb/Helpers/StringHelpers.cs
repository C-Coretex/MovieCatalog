namespace MovieCatalog.Providers.Omdb.Helpers
{
    internal static class StringHelpers
    {
        public static string FormatNullableString(this string? value)
        {
            return value ?? "N/A";
        }
    }
}
