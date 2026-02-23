using ProCalendar.Maui.Core.Enums;
using ProCalendar.Maui.Core.Interfaces;
using ProCalendar.Maui.Core.Models;
using ProCalendar.Maui.Core.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ProCalendar.Maui.Controls
{
    /// <summary>
    /// Main implementation file for ProCalendarView.
    /// Contains constructors and core rendering logic.
    /// </summary>
    [ContentProperty(nameof(DayTemplate))]
    public partial class ProCalendarView : TemplatedView
    {
        #region Private Fields

        private readonly ICalendarService _calendarService;
        private Grid? _mainGrid;
        private Grid? _headerGrid;
        private Grid? _daysGrid;
        private ScrollView? _scrollView;

        #endregion

        #region Events

        /// <summary>
        /// Fired when a date is selected.
        /// </summary>
        public event EventHandler<DateSelectedEventArgs>? DateSelected;

        /// <summary>
        /// Fired when a date range is selected.
        /// </summary>
        public event EventHandler<DateRangeSelectedEventArgs>? RangeSelected;

        /// <summary>
        /// Fired when the displayed month changes.
        /// </summary>
        public event EventHandler<MonthChangedEventArgs>? MonthChanged;

        /// <summary>
        /// Fired when an event is tapped.
        /// </summary>
        public event EventHandler<EventTappedEventArgs>? EventTapped;

        #endregion

        #region Constructors

        public ProCalendarView()
        {
            _calendarService = new CalendarService();
            
            SelectedDates = new ObservableCollection<DateTime>();

            BuildUI();
        }

        public ProCalendarView(ICalendarService calendarService) : this()
        {
            _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
        }

        #endregion

        #region UI Building

        private void BuildUI()
        {
            _mainGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }, // Header
                    new RowDefinition { Height = GridLength.Star }  // Days
                },
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            BuildHeader();
            BuildDaysGrid();
        }

        private void BuildHeader()
        {
            _headerGrid = new Grid
            {
                BackgroundColor = HeaderBackgroundColor,
                Padding = new Thickness(10),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            // Previous button
            var prevButton = new Button
            {
                Text = "◀",
                Command = PreviousCommand,
                BackgroundColor = Colors.Transparent
            };

            // Month/Year label
            var monthLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold
            };
            monthLabel.SetBinding(Label.TextProperty, new Binding(nameof(DisplayDate), source: this, 
                converter: new MonthYearConverter()));

            // Next button
            var nextButton = new Button
            {
                Text = "▶",
                Command = NextCommand,
                BackgroundColor = Colors.Transparent
            };

            _headerGrid.Add(prevButton, 0, 0);
            _headerGrid.Add(monthLabel, 1, 0);
            _headerGrid.Add(nextButton, 2, 0);

            _mainGrid?.Add(_headerGrid, 0, 0);
        }

        private void BuildDaysGrid()
        {
            _daysGrid = new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 1,
                BackgroundColor = Colors.LightGray
            };

            _mainGrid?.Add(_daysGrid, 0, 1);
            RenderCalendar();
        }

        #endregion

        #region Rendering

        private void RenderCalendar()
        {
            if (_daysGrid == null) return;

            _daysGrid.Clear();
            _daysGrid.RowDefinitions.Clear();
            _daysGrid.ColumnDefinitions.Clear();

            switch (ViewMode)
            {
                case CalendarViewMode.Month:
                    RenderMonthView();
                    break;
                case CalendarViewMode.Week:
                    RenderWeekView();
                    break;
                case CalendarViewMode.Day:
                    RenderDayView();
                    break;
                case CalendarViewMode.Agenda:
                    RenderAgendaView();
                    break;
            }
        }

        private void RenderMonthView()
        {
            if (_daysGrid == null) return;

            // Setup grid structure
            for (int i = 0; i < 8; i++) // 7 days + header
            {
                _daysGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            for (int i = 0; i < 7; i++)
            {
                _daysGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            // Render day headers
            RenderDayHeaders();

            // Get days for the month
            var days = _calendarService.GetDaysForMonth(DisplayDate.Year, DisplayDate.Month, Culture);

            // Render days
            int row = 1;
            int col = 0;

            foreach (var day in days)
            {
                var dayView = CreateDayView(day);
                _daysGrid.Add(dayView, col, row);

                col++;
                if (col >= 7)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private void RenderWeekView()
        {
            // Implementation for week view
        }

        private void RenderDayView()
        {
            // Implementation for day view
        }

        private void RenderAgendaView()
        {
            // Implementation for agenda view
        }

        private void RenderDayHeaders()
        {
            if (_daysGrid == null) return;

            var firstDayOfWeek = FirstDayOfWeek ?? _calendarService.GetFirstDayOfWeek(Culture);
            var dayNames = Culture.DateTimeFormat.AbbreviatedDayNames;

            for (int i = 0; i < 7; i++)
            {
                var dayIndex = ((int)firstDayOfWeek + i) % 7;
                var label = new Label
                {
                    Text = dayNames[dayIndex],
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold,
                    Padding = new Thickness(5)
                };

                _daysGrid.Add(label, i, 0);
            }
        }

        private View CreateDayView(CalendarDay day)
        {
            var template = GetDayTemplate(day);
            if (template != null)
            {
                var view = CreateViewFromTemplate(template, day);
                if (view != null) return view;
            }

            return CreateDefaultDayView(day);
        }

        private View CreateDefaultDayView(CalendarDay day)
        {
            var border = new Border
            {
                Padding = new Thickness(5),
                Stroke = Colors.Transparent,
                BackgroundColor = GetDayBackgroundColor(day)
            };

            var label = new Label
            {
                Text = day.Date.Day.ToString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = day.IsCurrentMonth ? DayTextColor : Colors.Gray,
                FontSize = DayFontSize
            };

            border.Content = label;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) => OnDayTapped(day);
            border.GestureRecognizers.Add(tapGesture);

            return border;
        }

        private Color GetDayBackgroundColor(CalendarDay day)
        {
            if (day.IsSelected)
                return SelectionColor;
            if (day.IsToday)
                return TodayColor.WithAlpha(0.3f);
            if (day.IsWeekend)
                return WeekendColor.WithAlpha(0.1f);
            
            return Colors.White;
        }

        #endregion

        #region Event Handlers

        private void OnDayTapped(CalendarDay day)
        {
            if (day.IsDisabled)
                return;

            switch (SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    SelectedDate = day.Date;
                    DateSelected?.Invoke(this, new DateSelectedEventArgs(day.Date));
                    ExecuteDateSelectedCommand(day.Date);
                    break;

                case CalendarSelectionMode.Multiple:
                    if (SelectedDates.Contains(day.Date))
                        SelectedDates.Remove(day.Date);
                    else
                        SelectedDates.Add(day.Date);
                    break;

                case CalendarSelectionMode.Range:
                    HandleRangeSelection(day.Date);
                    break;
            }

            RenderCalendar();
        }

        private void HandleRangeSelection(DateTime date)
        {
            if (SelectedRange == null || SelectedRange.EndDate != DateTime.MinValue)
            {
                SelectedRange = new DateRange { StartDate = date, EndDate = DateTime.MinValue };
            }
            else
            {
                if (date < SelectedRange.StartDate)
                {
                    SelectedRange.EndDate = SelectedRange.StartDate;
                    SelectedRange.StartDate = date;
                }
                else
                {
                    SelectedRange.EndDate = date;
                }

                RangeSelected?.Invoke(this, new DateRangeSelectedEventArgs(SelectedRange));
                ExecuteRangeSelectedCommand(SelectedRange);
            }
        }

        private void OnNext()
        {
            var oldDate = DisplayDate;
            
            DisplayDate = ViewMode switch
            {
                CalendarViewMode.Month => DisplayDate.AddMonths(1),
                CalendarViewMode.Week => DisplayDate.AddDays(7),
                CalendarViewMode.Day => DisplayDate.AddDays(1),
                _ => DisplayDate
            };

            MonthChanged?.Invoke(this, new MonthChangedEventArgs(oldDate, DisplayDate));
            ExecuteMonthChangedCommand(oldDate, DisplayDate);
        }

        private void OnPrevious()
        {
            var oldDate = DisplayDate;
            
            DisplayDate = ViewMode switch
            {
                CalendarViewMode.Month => DisplayDate.AddMonths(-1),
                CalendarViewMode.Week => DisplayDate.AddDays(-7),
                CalendarViewMode.Day => DisplayDate.AddDays(-1),
                _ => DisplayDate
            };

            MonthChanged?.Invoke(this, new MonthChangedEventArgs(oldDate, DisplayDate));
            ExecuteMonthChangedCommand(oldDate, DisplayDate);
        }

        private void OnToday()
        {
            DisplayDate = DateTime.Today;
        }

        #endregion

        #region Property Changed Handlers

        private static void OnDisplayDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnViewModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnSelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnSelectedDatesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnSelectedRangeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnCultureChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnFirstDayOfWeekChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnEventsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        private static void OnIsSwipeEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Handle swipe enabled/disabled
        }

        private static void OnDateRangeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ProCalendarView calendar)
            {
                calendar.RenderCalendar();
            }
        }

        #endregion
    }

    #region Event Args

    public class DateSelectedEventArgs : EventArgs
    {
        public DateTime SelectedDate { get; }
        public DateSelectedEventArgs(DateTime date) => SelectedDate = date;
    }

    public class DateRangeSelectedEventArgs : EventArgs
    {
        public DateRange SelectedRange { get; }
        public DateRangeSelectedEventArgs(DateRange range) => SelectedRange = range;
    }

    public class MonthChangedEventArgs : EventArgs
    {
        public DateTime OldDate { get; }
        public DateTime NewDate { get; }
        public MonthChangedEventArgs(DateTime oldDate, DateTime newDate)
        {
            OldDate = oldDate;
            NewDate = newDate;
        }
    }

    public class EventTappedEventArgs : EventArgs
    {
        public CalendarEvent Event { get; }
        public EventTappedEventArgs(CalendarEvent calendarEvent) => Event = calendarEvent;
    }

    #endregion

    #region Converters

    public class MonthYearConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("MMMM yyyy", culture ?? CultureInfo.CurrentCulture);
            }
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
