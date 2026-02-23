using ProCalendar.Maui.Core.Models;
using System.Windows.Input;

namespace ProCalendar.Maui.Controls
{
    /// <summary>
    /// Command-related properties and implementations for ProCalendarView.
    /// Enables full MVVM support.
    /// </summary>
    public partial class ProCalendarView
    {
        #region Command BindableProperties

        /// <summary>
        /// Command executed when a date is selected.
        /// CommandParameter: DateTime (selected date)
        /// </summary>
        public static readonly BindableProperty DateSelectedCommandProperty =
            BindableProperty.Create(
                nameof(DateSelectedCommand),
                typeof(ICommand),
                typeof(ProCalendarView),
                null);

        /// <summary>
        /// Command executed when a date range is selected.
        /// CommandParameter: DateRange
        /// </summary>
        public static readonly BindableProperty RangeSelectedCommandProperty =
            BindableProperty.Create(
                nameof(RangeSelectedCommand),
                typeof(ICommand),
                typeof(ProCalendarView),
                null);

        /// <summary>
        /// Command executed when the displayed month changes.
        /// CommandParameter: MonthChangedEventArgs
        /// </summary>
        public static readonly BindableProperty MonthChangedCommandProperty =
            BindableProperty.Create(
                nameof(MonthChangedCommand),
                typeof(ICommand),
                typeof(ProCalendarView),
                null);

        /// <summary>
        /// Command executed when an event is tapped.
        /// CommandParameter: CalendarEvent
        /// </summary>
        public static readonly BindableProperty EventTappedCommandProperty =
            BindableProperty.Create(
                nameof(EventTappedCommand),
                typeof(ICommand),
                typeof(ProCalendarView),
                null);

        /// <summary>
        /// Command executed when navigating to next month/week/day.
        /// </summary>
        public static readonly BindableProperty NextCommandProperty =
            BindableProperty.Create(
                nameof(NextCommand),
                typeof(ICommand),
                typeof(ProCalendarView),
                null,
                defaultValueCreator: bindable => new Command(() => ((ProCalendarView)bindable).OnNext()));

        /// <summary>
        /// Command executed when navigating to previous month/week/day.
        /// </summary>
        public static readonly BindableProperty PreviousCommandProperty =
            BindableProperty.Create(
                nameof(PreviousCommand),
                typeof(ICommand),
                typeof(ProCalendarView),
                null,
                defaultValueCreator: bindable => new Command(() => ((ProCalendarView)bindable).OnPrevious()));

        /// <summary>
        /// Command executed when navigating to today.
        /// </summary>
        public static readonly BindableProperty TodayCommandProperty =
            BindableProperty.Create(
                nameof(TodayCommand),
                typeof(ICommand),
                typeof(ProCalendarView),
                null,
                defaultValueCreator: bindable => new Command(() => ((ProCalendarView)bindable).OnToday()));

        #endregion

        #region Command Properties

        /// <summary>
        /// Gets or sets the command executed when a date is selected.
        /// </summary>
        public ICommand? DateSelectedCommand
        {
            get => (ICommand?)GetValue(DateSelectedCommandProperty);
            set => SetValue(DateSelectedCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command executed when a date range is selected.
        /// </summary>
        public ICommand? RangeSelectedCommand
        {
            get => (ICommand?)GetValue(RangeSelectedCommandProperty);
            set => SetValue(RangeSelectedCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command executed when the month changes.
        /// </summary>
        public ICommand? MonthChangedCommand
        {
            get => (ICommand?)GetValue(MonthChangedCommandProperty);
            set => SetValue(MonthChangedCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command executed when an event is tapped.
        /// </summary>
        public ICommand? EventTappedCommand
        {
            get => (ICommand?)GetValue(EventTappedCommandProperty);
            set => SetValue(EventTappedCommandProperty, value);
        }

        /// <summary>
        /// Gets the command for navigating to the next period.
        /// </summary>
        public ICommand NextCommand
        {
            get => (ICommand)GetValue(NextCommandProperty);
            set => SetValue(NextCommandProperty, value);
        }

        /// <summary>
        /// Gets the command for navigating to the previous period.
        /// </summary>
        public ICommand PreviousCommand
        {
            get => (ICommand)GetValue(PreviousCommandProperty);
            set => SetValue(PreviousCommandProperty, value);
        }

        /// <summary>
        /// Gets the command for navigating to today.
        /// </summary>
        public ICommand TodayCommand
        {
            get => (ICommand)GetValue(TodayCommandProperty);
            set => SetValue(TodayCommandProperty, value);
        }

        #endregion

        #region Command Execution Helpers

        private void ExecuteDateSelectedCommand(DateTime date)
        {
            if (DateSelectedCommand?.CanExecute(date) == true)
            {
                DateSelectedCommand.Execute(date);
            }
        }

        private void ExecuteRangeSelectedCommand(DateRange range)
        {
            if (RangeSelectedCommand?.CanExecute(range) == true)
            {
                RangeSelectedCommand.Execute(range);
            }
        }

        private void ExecuteMonthChangedCommand(DateTime oldDate, DateTime newDate)
        {
            var args = new MonthChangedEventArgs(oldDate, newDate);
            if (MonthChangedCommand?.CanExecute(args) == true)
            {
                MonthChangedCommand.Execute(args);
            }
        }

        private void ExecuteEventTappedCommand(CalendarEvent calendarEvent)
        {
            if (EventTappedCommand?.CanExecute(calendarEvent) == true)
            {
                EventTappedCommand.Execute(calendarEvent);
            }
        }

        #endregion
    }
}
