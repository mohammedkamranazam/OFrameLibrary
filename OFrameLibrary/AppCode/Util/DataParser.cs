using System;
using System.Globalization;

namespace OFrameLibrary.Util
{
    public static class DataParser
    {
        public static bool BoolParse(this string text)
        {
            return BoolParse(text, false);
        }

        public static bool BoolParse(this string text, bool returnTrue)
        {

            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            switch (text)
            {
                case "0":
                    return false;

                case "1":
                    return true;
            }

            if (!bool.TryParse(text, out var value))
            {
                value = returnTrue;

                ErrorLogger.LogError($"BoolParse: string [{text}] not in proper format");
            }

            return value;
        }

        public static DateTime DateTimeParse(this string text)
        {

            if (!DateTime.TryParse(text, out var value))
            {
                ErrorLogger.LogError($"DateTimeParse: string [{text}] not in proper format");
            }

            return value;
        }

        public static double DoubleParse(this string number)
        {
            return DoubleParse(number, 0);
        }

        public static double DoubleParse(this string number, double nullReturn)
        {

            if (!double.TryParse(number, out var value))
            {
                value = nullReturn;

                ErrorLogger.LogError($"DoubleParse: string [{number}] not in proper format");
            }

            return value;
        }

        public static float FloatParse(this string number)
        {
            return FloatParse(number, 0);
        }

        public static float FloatParse(this string number, float nullReturn)
        {

            if (!float.TryParse(number, out var value))
            {
                value = nullReturn;

                ErrorLogger.LogError($"FloatParse: string [{number}] not in proper format");
            }

            return value;
        }

        public static string GetDateFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString(Validator.DateParseExpression);
        }

        public static string GetDateFormattedString(this DateTime? dateTime)
        {
            var value = string.Empty;

            if (dateTime.HasValue)
            {
                var dateTimeValue = (DateTime)dateTime;

                value = GetDateFormattedString(dateTimeValue);
            }

            return value;
        }

        public static string GetDateTimeFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString(Validator.DateTimeParseExpression);
        }

        public static string GetDateTimeFormattedString(this DateTime? dateTime)
        {
            var value = string.Empty;

            if (dateTime.HasValue)
            {
                var dateTimeValue = (DateTime)dateTime;

                value = GetDateTimeFormattedString(dateTimeValue);
            }

            return value;
        }

        public static string GetTimeFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString(Validator.TimeParseExpression);
        }

        public static string GetTimeFormattedString(this DateTime? dateTime)
        {
            var value = string.Empty;

            if (dateTime.HasValue)
            {
                var dateTimeValue = (DateTime)dateTime;

                value = GetTimeFormattedString(dateTimeValue);
            }

            return value;
        }

        public static int IntParse(this string number)
        {
            return IntParse(number, 0);
        }

        public static int IntParse(this string number, int nullReturn)
        {

            var dotPos = number.IndexOf('.');

            if (dotPos > 0)
            {
                number = number.Remove(dotPos);
            }

            if (!int.TryParse(number, out var value))
            {
                value = nullReturn;

                ErrorLogger.LogError($"IntParse: string [{number}] not in proper format");
            }

            return value;
        }

        public static long LongParse(this string number)
        {
            return LongParse(number, 0);
        }

        public static long LongParse(this string number, long nullReturn)
        {

            if (!long.TryParse(number, out var value))
            {
                value = nullReturn;

                ErrorLogger.LogError($"LongParse: string [{number}] not in proper format");
            }

            return value;
        }

        public static DateTime? NullableDateTimeParse(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }


            if (!DateTime.TryParse(text, out var value))
            {
                ErrorLogger.LogError($"NullableDateTimeParse: string [{text}] not in proper format");
                return null;
            }

            return value;
        }

        public static double? NullableDoubleParse(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return DoubleParse(number, 0);
        }

        public static float? NullableFloatParse(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return FloatParse(number, 0);
        }

        public static int? NullableIntParse(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return IntParse(number, 0);
        }

        public static long? NullableLongParse(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return LongParse(number, 0);
        }

        public static bool StringToDate(string date, out DateTime dob, string format = "MM/dd/yyyy")
        {
            return DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
        }
    }
}
