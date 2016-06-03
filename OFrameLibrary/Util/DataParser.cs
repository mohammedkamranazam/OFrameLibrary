using System;

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
            bool value;

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

            if (!bool.TryParse(text, out value))
            {
                value = returnTrue;

                ErrorLogger.LogError(string.Format("BoolParse: string [{0}] not in proper format", text));
            }

            return value;
        }

        public static DateTime DateTimeParse(this string text)
        {
            DateTime value;

            if (!DateTime.TryParse(text, out value))
            {
                ErrorLogger.LogError(string.Format("DateTimeParse: string [{0}] not in proper format", text));
            }

            return value;
        }

        public static double DoubleParse(this string number)
        {
            return DoubleParse(number, 0);
        }

        public static double DoubleParse(this string number, double nullReturn)
        {
            double value;

            if (!double.TryParse(number, out value))
            {
                value = nullReturn;

                ErrorLogger.LogError(string.Format("DoubleParse: string [{0}] not in proper format", number));
            }

            return value;
        }

        public static float FloatParse(this string number)
        {
            return FloatParse(number, 0);
        }

        public static float FloatParse(this string number, float nullReturn)
        {
            float value;

            if (!float.TryParse(number, out value))
            {
                value = nullReturn;

                ErrorLogger.LogError(string.Format("FloatParse: string [{0}] not in proper format", number));
            }

            return value;
        }

        public static string GetDateFormattedString(this DateTime dateTime)
        {
            string value = dateTime.ToString(Validator.DateParseExpression);

            return value;
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
            string value = dateTime.ToString(Validator.DateTimeParseExpression);

            return value;
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
            string value = dateTime.ToString(Validator.TimeParseExpression);

            return value;
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
            int value;

            var dotPos = number.IndexOf('.');

            if (dotPos > 0)
            {
                number = number.Remove(dotPos);
            }

            if (!int.TryParse(number, out value))
            {
                value = nullReturn;

                ErrorLogger.LogError(string.Format("IntParse: string [{0}] not in proper format", number));
            }

            return value;
        }

        public static long LongParse(this string number)
        {
            return LongParse(number, 0);
        }

        public static long LongParse(this string number, long nullReturn)
        {
            long value;

            if (!long.TryParse(number, out value))
            {
                value = nullReturn;

                ErrorLogger.LogError(string.Format("LongParse: string [{0}] not in proper format", number));
            }

            return value;
        }

        public static DateTime? NullableDateTimeParse(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            DateTime value;

            if (!DateTime.TryParse(text, out value))
            {
                ErrorLogger.LogError(string.Format("NullableDateTimeParse: string [{0}] not in proper format", text));
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
    }
}