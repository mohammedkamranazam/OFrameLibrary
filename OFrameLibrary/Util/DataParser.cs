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
                value = (returnTrue) ? true : false;

                ErrorLogger.LogError(String.Format("BoolParse: string [{0}] not in proper format", text));
            }

            return value;
        }

        public static DateTime DateTimeParse(this string text)
        {
            DateTime value;

            if (!DateTime.TryParse(text, out value))
            {
                ErrorLogger.LogError(String.Format("DateTimeParse: string [{0}] not in proper format", text));
            }

            return value;
        }

        public static double DoubleParse(this string number)
        {
            return DoubleParse(number, false);
        }

        public static double DoubleParse(this string number, bool returnOne)
        {
            double value;

            if (!double.TryParse(number, out value))
            {
                value = (returnOne) ? 1 : 0;

                ErrorLogger.LogError(String.Format("DoubleParse: string [{0}] not in proper format", number));
            }

            return value;
        }

        public static float FloatParse(this string number)
        {
            return FloatParse(number, false);
        }

        public static float FloatParse(this string number, bool returnOne)
        {
            float value;

            if (!float.TryParse(number, out value))
            {
                value = (returnOne) ? 1 : 0;

                ErrorLogger.LogError(String.Format("FloatParse: string [{0}] not in proper format", number));
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
            return IntParse(number, false);
        }

        public static int IntParse(this string number, bool returnOne)
        {
            int value;

            var dotPos = number.IndexOf('.');

            if (dotPos > 0)
            {
                number = number.Remove(dotPos);
            }

            if (!int.TryParse(number, out value))
            {
                value = (returnOne) ? 1 : 0;

                ErrorLogger.LogError(String.Format("IntParse: string [{0}] not in proper format", number));
            }

            return value;
        }

        public static long LongParse(this string number)
        {
            return LongParse(number, false);
        }

        public static long LongParse(this string number, bool returnOne)
        {
            long value;

            if (!long.TryParse(number, out value))
            {
                value = (returnOne) ? 1 : 0;

                ErrorLogger.LogError(String.Format("LongParse: string [{0}] not in proper format", number));
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
                ErrorLogger.LogError(String.Format("NullableDateTimeParse: string [{0}] not in proper format", text));
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

            return DoubleParse(number, false);
        }

        public static float? NullableFloatParse(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return FloatParse(number, false);
        }

        public static int? NullableIntParse(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return IntParse(number, false);
        }

        public static long? NullableLongParse(this string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return LongParse(number, false);
        }
    }
}