namespace vnpost_ocr_system.Models
{
    public static class IntegerExtensions
    {
        public static int ParseInt(this string value, int defaultIntValue = -1)
        {
            int parsedInt;
            if (int.TryParse(value, out parsedInt))
            {
                return parsedInt;
            }

            return defaultIntValue;
        }

        public static int? ParseNullableInt(this string value)
        {
            if (string.IsNullOrEmpty(value) || value == "null")
            {
                return null;
            }

            return value.ParseInt();
        }
    }
}