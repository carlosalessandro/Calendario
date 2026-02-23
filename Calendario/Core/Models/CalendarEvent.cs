using ProCalendar.Maui.Core.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProCalendar.Maui.Core.Models
{
    /// <summary>
    /// Represents a calendar event with full support for recurrence and customization.
    /// </summary>
    public class CalendarEvent : INotifyPropertyChanged
    {
        private string _id = string.Empty;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _isAllDay;
        private Color _color = Colors.Blue;
        private RecurrenceType _recurrenceType;
        private object? _data;

        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// Event title/summary.
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Detailed description of the event.
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        /// <summary>
        /// Start date and time of the event.
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        /// <summary>
        /// End date and time of the event.
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        /// <summary>
        /// Indicates if this is an all-day event.
        /// </summary>
        public bool IsAllDay
        {
            get => _isAllDay;
            set => SetProperty(ref _isAllDay, value);
        }

        /// <summary>
        /// Color used to display the event.
        /// </summary>
        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        /// <summary>
        /// Recurrence pattern for the event.
        /// </summary>
        public RecurrenceType RecurrenceType
        {
            get => _recurrenceType;
            set => SetProperty(ref _recurrenceType, value);
        }

        /// <summary>
        /// Custom data attached to the event for extensibility.
        /// </summary>
        public object? Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
