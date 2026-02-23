using ProCalendar.Maui.Core.Interfaces;
using ProCalendar.Maui.Core.Models;
using System.Collections.Concurrent;

namespace ProCalendar.Maui.Core.Services
{
    /// <summary>
    /// In-memory event provider for testing and simple scenarios.
    /// For production, implement IEventProvider with database or API integration.
    /// </summary>
    public class InMemoryEventProvider : IEventProvider
    {
        private readonly ConcurrentDictionary<string, CalendarEvent> _events = new();

        public Task<IEnumerable<CalendarEvent>> LoadEventsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var events = _events.Values
                .Where(e => e.StartDate.Date <= endDate.Date && e.EndDate.Date >= startDate.Date)
                .OrderBy(e => e.StartDate)
                .ToList();

            return Task.FromResult<IEnumerable<CalendarEvent>>(events);
        }

        public Task<CalendarEvent> AddEventAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(calendarEvent.Id))
            {
                calendarEvent.Id = Guid.NewGuid().ToString();
            }

            _events[calendarEvent.Id] = calendarEvent;
            return Task.FromResult(calendarEvent);
        }

        public Task<CalendarEvent> UpdateEventAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
        {
            if (_events.ContainsKey(calendarEvent.Id))
            {
                _events[calendarEvent.Id] = calendarEvent;
                return Task.FromResult(calendarEvent);
            }

            throw new KeyNotFoundException($"Event with ID {calendarEvent.Id} not found.");
        }

        public Task<bool> DeleteEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_events.TryRemove(eventId, out _));
        }
    }
}
