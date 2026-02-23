using System.Collections.ObjectModel;

namespace ProCalendar.Maui.Core.Models
{
    /// <summary>
    /// Represents a single day in the calendar with its associated events and state.
    /// </summary>
    public class CalendarDay
    {
        /// <summary>
        /// The date this day represents.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Indicates if this day is in the current month being displayed.
        /// </summary>
        public bool IsCurrentMonth { get; set; }

        /// <summary>
        /// Indicates if this day is today.
        /// </summary>
        public bool IsToday { get; set; }

        /// <summary>
        /// Indicates if this day is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Indicates if this day is part of a selected range.
        /// </summary>
        public bool IsInRange { get; set; }

        /// <summary>
        /// Indicates if this day is the start of a selected range.
        /// </summary>
        public bool IsRangeStart { get; set; }

        /// <summary>
        /// Indicates if this day is the end of a selected range.
        /// </summary>
        public bool IsRangeEnd { get; set; }

        /// <summary>
        /// Indicates if this day is a weekend.
        /// </summary>
        public bool IsWeekend { get; set; }

        /// <summary>
        /// Indicates if this day is disabled/not selectable.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Events occurring on this day.
        /// </summary>
        public ObservableCollection<CalendarEvent> Events { get; set; } = new();

        /// <summary>
        /// Number of events on this day (for performance when not loading full event list).
        /// </summary>
        public int EventCount => Events?.Count ?? 0;

        /// <summary>
        /// Custom data for extensibility.
        /// </summary>
        public object? Data { get; set; }
    }
}
