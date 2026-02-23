using ProCalendar.Maui.Core.Models;

namespace ProCalendar.Maui.Core.Interfaces
{
    /// <summary>
    /// Interface for providing calendar events from various sources.
    /// Implement this to integrate with external calendars (Google, Outlook, etc.)
    /// </summary>
    public interface IEventProvider
    {
        /// <summary>
        /// Loads events for the specified date range.
        /// </summary>
        Task<IEnumerable<CalendarEvent>> LoadEventsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new event.
        /// </summary>
        Task<CalendarEvent> AddEventAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        Task<CalendarEvent> UpdateEventAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an event.
        /// </summary>
        Task<bool> DeleteEventAsync(string eventId, CancellationToken cancellationToken = default);
    }
}
