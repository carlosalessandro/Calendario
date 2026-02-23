using ProCalendar.Maui.Core.Models;
using System.Globalization;

namespace ProCalendar.Maui.Core.Interfaces
{
    /// <summary>
    /// Core service interface for calendar operations and date calculations.
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// Gets the days to display for a given month.
        /// </summary>
        List<CalendarDay> GetDaysForMonth(int year, int month, CultureInfo culture);

        /// <summary>
        /// Gets the days to display for a given week.
        /// </summary>
        List<CalendarDay> GetDaysForWeek(DateTime date, CultureInfo culture);

        /// <summary>
        /// Gets the first day of the week based on culture.
        /// </summary>
        DayOfWeek GetFirstDayOfWeek(CultureInfo culture);

        /// <summary>
        /// Checks if a date is a weekend based on culture.
        /// </summary>
        bool IsWeekend(DateTime date, CultureInfo culture);

        /// <summary>
        /// Gets the week number for a date.
        /// </summary>
        int GetWeekNumber(DateTime date, CultureInfo culture);

        /// <summary>
        /// Gets events for a specific date range.
        /// </summary>
        Task<IEnumerable<CalendarEvent>> GetEventsAsync(DateTime startDate, DateTime endDate);
    }
}
