namespace vnpost_ocr_system.Models
{
    public static class LongExtensions
    {
        public static long ParseLong(this string value, long defaultLongValue = 0)
        {
            long parsedLong;
            if (long.TryParse(value, out parsedLong))
            {
                return parsedLong;
            }

            return defaultLongValue;
        }

        public static long? ParseNullableLong(this string value)
        {
            if (string.IsNullOrEmpty(value) || value == "null")
                return null;

            return value.ParseLong();
        }
    }
}