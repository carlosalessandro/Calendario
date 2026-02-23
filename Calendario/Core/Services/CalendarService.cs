using ProCalendar.Maui.Core.Interfaces;
using ProCalendar.Maui.Core.Models;
using System.Globalization;

namespace ProCalendar.Maui.Core.Services
{
    /// <summary>
    /// Core calendar service implementing date calculations and calendar logic.
    /// Optimized for performance with minimal allocations.
    /// </summary>
    public class CalendarService : ICalendarService
    {
        private readonly IEventProvider? _eventProvider;

        public CalendarService(IEventProvider? eventProvider = null)
        {
            _eventProvider = eventProvider;
        }

        /// <inheritdoc/>
        public List<CalendarDay> GetDaysForMonth(int year, int month, CultureInfo culture)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var firstDayOfWeek = GetFirstDayOfWeek(culture);

            // Calculate the starting day (may be from previous month)
            var startDate = firstDayOfMonth;
            while (startDate.DayOfWeek != firstDayOfWeek)
            {
                startDate = startDate.AddDays(-1);
            }

            // Calculate total days to display (typically 35 or 42 days)
            var daysToDisplay = 42; // 6 weeks
            var days = new List<CalendarDay>(daysToDisplay);
            var today = DateTime.Today;

            for (int i = 0; i < daysToDisplay; i++)
            {
                var currentDate = startDate.AddDays(i);
                var day = new CalendarDay
                {
                    Date = currentDate,
                    IsCurrentMonth = currentDate.Month == month && currentDate.Year == year,
                    IsToday = currentDate.Date == today,
                    IsWeekend = IsWeekend(currentDate, culture),
                    IsDisabled = false
                };

                days.Add(day);
            }

            return days;
        }

        /// <inheritdoc/>
        public List<CalendarDay> GetDaysForWeek(DateTime date, CultureInfo culture)
        {
            var firstDayOfWeek = GetFirstDayOfWeek(culture);
            var startDate = date;

            // Find the start of the week
            while (startDate.DayOfWeek != firstDayOfWeek)
            {
                startDate = startDate.AddDays(-1);
            }

            var days = new List<CalendarDay>(7);
            var today = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                var currentDate = startDate.AddDays(i);
                var day = new CalendarDay
                {
                    Date = currentDate,
                    IsCurrentMonth = true,
                    IsToday = currentDate.Date == today,
                    IsWeekend = IsWeekend(currentDate, culture),
                    IsDisabled = false
                };

                days.Add(day);
            }

            return days;
        }

        /// <inheritdoc/>
        public DayOfWeek GetFirstDayOfWeek(CultureInfo culture)
        {
            return culture?.DateTimeFormat.FirstDayOfWeek ?? DayOfWeek.Sunday;
        }

        /// <inheritdoc/>
        public bool IsWeekend(DateTime date, CultureInfo culture)
        {
            // Most cultures: Saturday and Sunday
            // Can be extended for cultures with different weekend days
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <inheritdoc/>
        public int GetWeekNumber(DateTime date, CultureInfo culture)
        {
            var calendar = culture?.Calendar ?? CultureInfo.CurrentCulture.Calendar;
            var dateTimeFormat = culture?.DateTimeFormat ?? CultureInfo.CurrentCulture.DateTimeFormat;
            
            return calendar.GetWeekOfYear(
                date,
                dateTimeFormat.CalendarWeekRule,
                dateTimeFormat.FirstDayOfWeek
            );
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CalendarEvent>> GetEventsAsync(DateTime startDate, DateTime endDate)
        {
            if (_eventProvider == null)
                return Enumerable.Empty<CalendarEvent>();

            return await _eventProvider.LoadEventsAsync(startDate, endDate);
        }
    }
}
