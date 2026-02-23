using ProCalendar.Maui.Controls;
using ProCalendar.Maui.Core.Enums;
using ProCalendar.Maui.Core.Interfaces;

namespace ProCalendar.Maui.Extensions
{
    /// <summary>
    /// Fluent API extensions for configuring ProCalendar.
    /// </summary>
    public static class ProCalendarExtensions
    {
        /// <summary>
        /// Configures the calendar with a fluent API.
        /// </summary>
        public static ProCalendarView Configure(this ProCalendarView calendar, Action<CalendarConfiguration> configure)
        {
            var config = new CalendarConfiguration(calendar);
            configure(config);
            return calendar;
        }

        /// <summary>
        /// Sets the view mode.
        /// </summary>
        public static ProCalendarView WithViewMode(this ProCalendarView calendar, CalendarViewMode mode)
        {
            calendar.ViewMode = mode;
            return calendar;
        }

        /// <summary>
        /// Sets the selection mode.
        /// </summary>
        public static ProCalendarView WithSelectionMode(this ProCalendarView calendar, CalendarSelectionMode mode)
        {
            calendar.SelectionMode = mode;
            return calendar;
        }

        /// <summary>
        /// Sets custom colors.
        /// </summary>
        public static ProCalendarView WithColors(this ProCalendarView calendar, 
            Color? todayColor = null, 
            Color? selectionColor = null, 
            Color? weekendColor = null)
        {
            if (todayColor != null) calendar.TodayColor = todayColor;
            if (selectionColor != null) calendar.SelectionColor = selectionColor;
            if (weekendColor != null) calendar.WeekendColor = weekendColor;
            return calendar;
        }

        /// <summary>
        /// Sets date range limits.
        /// </summary>
        public static ProCalendarView WithDateRange(this ProCalendarView calendar, DateTime? minDate, DateTime? maxDate)
        {
            calendar.MinimumDate = minDate;
            calendar.MaximumDate = maxDate;
            return calendar;
        }

        /// <summary>
        /// Enables week numbers.
        /// </summary>
        public static ProCalendarView ShowWeekNumbers(this ProCalendarView calendar, bool show = true)
        {
            calendar.ShowWeekNumbers = show;
            return calendar;
        }
    }

    /// <summary>
    /// Configuration builder for ProCalendar.
    /// </summary>
    public class CalendarConfiguration
    {
        private readonly ProCalendarView _calendar;

        public CalendarConfiguration(ProCalendarView calendar)
        {
            _calendar = calendar;
        }

        public CalendarConfiguration ViewMode(CalendarViewMode mode)
        {
            _calendar.ViewMode = mode;
            return this;
        }

        public CalendarConfiguration SelectionMode(CalendarSelectionMode mode)
        {
            _calendar.SelectionMode = mode;
            return this;
        }

        public CalendarConfiguration TodayColor(Color color)
        {
            _calendar.TodayColor = color;
            return this;
        }

        public CalendarConfiguration SelectionColor(Color color)
        {
            _calendar.SelectionColor = color;
            return this;
        }

        public CalendarConfiguration WeekendColor(Color color)
        {
            _calendar.WeekendColor = color;
            return this;
        }

        public CalendarConfiguration DateRange(DateTime? minDate, DateTime? maxDate)
        {
            _calendar.MinimumDate = minDate;
            _calendar.MaximumDate = maxDate;
            return this;
        }

        public CalendarConfiguration Culture(System.Globalization.CultureInfo culture)
        {
            _calendar.Culture = culture;
            return this;
        }

        public CalendarConfiguration DayTemplate(DataTemplate template)
        {
            _calendar.DayTemplate = template;
            return this;
        }

        public CalendarConfiguration EventTemplate(DataTemplate template)
        {
            _calendar.EventTemplate = template;
            return this;
        }
    }
}
