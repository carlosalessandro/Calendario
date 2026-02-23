using ProCalendar.Maui.Core.Enums;
using ProCalendar.Maui.Core.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ProCalendar.Maui.Controls
{
    /// <summary>
    /// BindableProperty definitions for ProCalendarView.
    /// All properties are XAML-bindable and support TwoWay binding where appropriate.
    /// </summary>
    public partial class ProCalendarView
    {
        #region Core BindableProperties

        /// <summary>
        /// The currently displayed date (month/week/day).
        /// </summary>
        public static readonly BindableProperty DisplayDateProperty =
            BindableProperty.Create(
                nameof(DisplayDate),
                typeof(DateTime),
                typeof(ProCalendarView),
                DateTime.Today,
                BindingMode.TwoWay,
                propertyChanged: OnDisplayDateChanged);

        /// <summary>
        /// The view mode (Month, Week, Day, Agenda).
        /// </summary>
        public static readonly BindableProperty ViewModeProperty =
            BindableProperty.Create(
                nameof(ViewMode),
                typeof(CalendarViewMode),
                typeof(ProCalendarView),
                CalendarViewMode.Month,
                propertyChanged: OnViewModeChanged);

        /// <summary>
        /// The selection mode (None, Single, Multiple, Range).
        /// </summary>
        public static readonly BindableProperty SelectionModeProperty =
            BindableProperty.Create(
                nameof(SelectionMode),
                typeof(CalendarSelectionMode),
                typeof(ProCalendarView),
                CalendarSelectionMode.Single,
                propertyChanged: OnSelectionModeChanged);

        /// <summary>
        /// The currently selected date (for Single selection mode).
        /// </summary>
        public static readonly BindableProperty SelectedDateProperty =
            BindableProperty.Create(
                nameof(SelectedDate),
                typeof(DateTime?),
                typeof(ProCalendarView),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedDateChanged);

        /// <summary>
        /// Collection of selected dates (for Multiple selection mode).
        /// </summary>
        public static readonly BindableProperty SelectedDatesProperty =
            BindableProperty.Create(
                nameof(SelectedDates),
                typeof(ObservableCollection<DateTime>),
                typeof(ProCalendarView),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedDatesChanged,
                defaultValueCreator: bindable => new ObservableCollection<DateTime>());

        /// <summary>
        /// The selected date range (for Range selection mode).
        /// </summary>
        public static readonly BindableProperty SelectedRangeProperty =
            BindableProperty.Create(
                nameof(SelectedRange),
                typeof(DateRange),
                typeof(ProCalendarView),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedRangeChanged);

        #endregion

        #region Data Source Properties

        /// <summary>
        /// The source collection of events to display.
        /// Supports IEnumerable, ObservableCollection, and INotifyCollectionChanged.
        /// </summary>
        public static readonly BindableProperty EventsSourceProperty =
            BindableProperty.Create(
                nameof(EventsSource),
                typeof(IEnumerable),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnEventsSourceChanged);

        #endregion

        #region Culture and Localization Properties

        /// <summary>
        /// The culture for date formatting and first day of week.
        /// </summary>
        public static readonly BindableProperty CultureProperty =
            BindableProperty.Create(
                nameof(Culture),
                typeof(CultureInfo),
                typeof(ProCalendarView),
                CultureInfo.CurrentCulture,
                propertyChanged: OnCultureChanged);

        /// <summary>
        /// The first day of the week. If null, uses culture default.
        /// </summary>
        public static readonly BindableProperty FirstDayOfWeekProperty =
            BindableProperty.Create(
                nameof(FirstDayOfWeek),
                typeof(DayOfWeek?),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnFirstDayOfWeekChanged);

        #endregion

        #region Visual Customization Properties

        /// <summary>
        /// Color for today's date.
        /// </summary>
        public static readonly BindableProperty TodayColorProperty =
            BindableProperty.Create(
                nameof(TodayColor),
                typeof(Color),
                typeof(ProCalendarView),
                Colors.Blue,
                propertyChanged: OnVisualPropertyChanged);

        /// <summary>
        /// Color for selected dates.
        /// </summary>
        public static readonly BindableProperty SelectionColorProperty =
            BindableProperty.Create(
                nameof(SelectionColor),
                typeof(Color),
                typeof(ProCalendarView),
                Colors.LightBlue,
                propertyChanged: OnVisualPropertyChanged);

        /// <summary>
        /// Color for weekend dates.
        /// </summary>
        public static readonly BindableProperty WeekendColorProperty =
            BindableProperty.Create(
                nameof(WeekendColor),
                typeof(Color),
                typeof(ProCalendarView),
                Colors.LightGray,
                propertyChanged: OnVisualPropertyChanged);

        /// <summary>
        /// Background color for the header.
        /// </summary>
        public static readonly BindableProperty HeaderBackgroundColorProperty =
            BindableProperty.Create(
                nameof(HeaderBackgroundColor),
                typeof(Color),
                typeof(ProCalendarView),
                Colors.Transparent,
                propertyChanged: OnVisualPropertyChanged);

        /// <summary>
        /// Text color for day numbers.
        /// </summary>
        public static readonly BindableProperty DayTextColorProperty =
            BindableProperty.Create(
                nameof(DayTextColor),
                typeof(Color),
                typeof(ProCalendarView),
                Colors.Black,
                propertyChanged: OnVisualPropertyChanged);

        /// <summary>
        /// Font size for day numbers.
        /// </summary>
        public static readonly BindableProperty DayFontSizeProperty =
            BindableProperty.Create(
                nameof(DayFontSize),
                typeof(double),
                typeof(ProCalendarView),
                14.0,
                propertyChanged: OnVisualPropertyChanged);

        #endregion

        #region Feature Properties

        /// <summary>
        /// Whether to show week numbers.
        /// </summary>
        public static readonly BindableProperty ShowWeekNumbersProperty =
            BindableProperty.Create(
                nameof(ShowWeekNumbers),
                typeof(bool),
                typeof(ProCalendarView),
                false,
                propertyChanged: OnLayoutPropertyChanged);

        /// <summary>
        /// Whether swipe navigation is enabled.
        /// </summary>
        public static readonly BindableProperty IsSwipeEnabledProperty =
            BindableProperty.Create(
                nameof(IsSwipeEnabled),
                typeof(bool),
                typeof(ProCalendarView),
                true,
                propertyChanged: OnIsSwipeEnabledChanged);

        /// <summary>
        /// Minimum selectable date.
        /// </summary>
        public static readonly BindableProperty MinimumDateProperty =
            BindableProperty.Create(
                nameof(MinimumDate),
                typeof(DateTime?),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnDateRangeChanged);

        /// <summary>
        /// Maximum selectable date.
        /// </summary>
        public static readonly BindableProperty MaximumDateProperty =
            BindableProperty.Create(
                nameof(MaximumDate),
                typeof(DateTime?),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnDateRangeChanged);

        /// <summary>
        /// Whether to show events in day cells.
        /// </summary>
        public static readonly BindableProperty ShowEventsProperty =
            BindableProperty.Create(
                nameof(ShowEvents),
                typeof(bool),
                typeof(ProCalendarView),
                true,
                propertyChanged: OnLayoutPropertyChanged);

        /// <summary>
        /// Maximum number of events to show per day cell.
        /// </summary>
        public static readonly BindableProperty MaxEventsPerDayProperty =
            BindableProperty.Create(
                nameof(MaxEventsPerDay),
                typeof(int),
                typeof(ProCalendarView),
                3,
                propertyChanged: OnLayoutPropertyChanged);

        #endregion

        #region Public Properties

        public DateTime DisplayDate
        {
            get => (DateTime)GetValue(DisplayDateProperty);
            set => SetValue(DisplayDateProperty, value);
        }

        public CalendarViewMode ViewMode
        {
            get => (CalendarViewMode)GetValue(ViewModeProperty);
            set => SetValue(ViewModeProperty, value);
        }

        public CalendarSelectionMode SelectionMode
        {
            get => (CalendarSelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        public ObservableCollection<DateTime> SelectedDates
        {
            get => (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty);
            set => SetValue(SelectedDatesProperty, value);
        }

        public DateRange? SelectedRange
        {
            get => (DateRange?)GetValue(SelectedRangeProperty);
            set => SetValue(SelectedRangeProperty, value);
        }

        public IEnumerable? EventsSource
        {
            get => (IEnumerable?)GetValue(EventsSourceProperty);
            set => SetValue(EventsSourceProperty, value);
        }

        public CultureInfo Culture
        {
            get => (CultureInfo)GetValue(CultureProperty);
            set => SetValue(CultureProperty, value);
        }

        public DayOfWeek? FirstDayOfWeek
        {
            get => (DayOfWeek?)GetValue(FirstDayOfWeekProperty);
            set => SetValue(FirstDayOfWeekProperty, value);
        }

        public Color TodayColor
        {
            get => (Color)GetValue(TodayColorProperty);
            set => SetValue(TodayColorProperty, value);
        }

        public Color SelectionColor
        {
            get => (Color)GetValue(SelectionColorProperty);
            set => SetValue(SelectionColorProperty, value);
        }

        public Color WeekendColor
        {
            get => (Color)GetValue(WeekendColorProperty);
            set => SetValue(WeekendColorProperty, value);
        }

        public Color HeaderBackgroundColor
        {
            get => (Color)GetValue(HeaderBackgroundColorProperty);
            set => SetValue(HeaderBackgroundColorProperty, value);
        }

        public Color DayTextColor
        {
            get => (Color)GetValue(DayTextColorProperty);
            set => SetValue(DayTextColorProperty, value);
        }

        public double DayFontSize
        {
            get => (double)GetValue(DayFontSizeProperty);
            set => SetValue(DayFontSizeProperty, value);
        }

        public bool ShowWeekNumbers
        {
            get => (bool)GetValue(ShowWeekNumbersProperty);
            set => SetValue(ShowWeekNumbersProperty, value);
        }

        public bool IsSwipeEnabled
        {
            get => (bool)GetValue(IsSwipeEnabledProperty);
            set => SetValue(IsSwipeEnabledProperty, value);
        }

        public DateTime? MinimumDate
        {
            get => (DateTime?)GetValue(MinimumDateProperty);
            set => SetValue(MinimumDateProperty, value);
        }

        public DateTime? MaximumDate
        {
            get => (DateTime?)GetValue(MaximumDateProperty);
            set => SetValue(MaximumDateProperty, value);
        }

        public bool ShowEvents
        {
            get => (bool)GetValue(ShowEventsProperty);
            set => SetValue(ShowEventsProperty, value);
        }

        public int MaxEventsPerDay
        {
            get => (int)GetValue(MaxEventsPerDayProperty);
            set => SetValue(MaxEventsPerDayProperty, value);
        }

        #endregion
    }
}
