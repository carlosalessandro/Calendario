using System.Globalization;

namespace ProCalendar.Maui.Converters
{
    /// <summary>
    /// Converts DateTime to formatted string based on culture.
    /// </summary>
    public class DateFormatConverter : IValueConverter
    {
        public string Format { get; set; } = "d";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                var format = parameter as string ?? Format;
                return date.ToString(format, culture ?? CultureInfo.CurrentCulture);
            }

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && DateTime.TryParse(str, culture, DateTimeStyles.None, out var date))
            {
                return date;
            }

            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// Converts boolean to color for visual states.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; } = Colors.Blue;
        public Color FalseColor { get; set; } = Colors.Transparent;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueColor : FalseColor;
            }

            return FalseColor;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts event count to visibility.
    /// </summary>
    public class EventCountToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count > 0;
            }

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
