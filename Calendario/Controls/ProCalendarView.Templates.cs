namespace ProCalendar.Maui.Controls
{
    /// <summary>
    /// Template-related properties and implementations for ProCalendarView.
    /// Enables full customization via DataTemplates and ControlTemplate.
    /// </summary>
    public partial class ProCalendarView
    {
        #region Template Part Names

        private const string PART_MainGrid = "PART_MainGrid";
        private const string PART_HeaderGrid = "PART_HeaderGrid";
        private const string PART_DaysGrid = "PART_DaysGrid";
        private const string PART_ScrollView = "PART_ScrollView";

        #endregion

        #region Template BindableProperties

        /// <summary>
        /// DataTemplate for rendering individual day cells.
        /// BindingContext: CalendarDay
        /// </summary>
        public static readonly BindableProperty DayTemplateProperty =
            BindableProperty.Create(
                nameof(DayTemplate),
                typeof(DataTemplate),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnTemplateChanged);

        /// <summary>
        /// DataTemplate for rendering the calendar header (month/year display).
        /// BindingContext: ProCalendarView
        /// </summary>
        public static readonly BindableProperty HeaderTemplateProperty =
            BindableProperty.Create(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnTemplateChanged);

        /// <summary>
        /// DataTemplate for rendering events within day cells.
        /// BindingContext: CalendarEvent
        /// </summary>
        public static readonly BindableProperty EventTemplateProperty =
            BindableProperty.Create(
                nameof(EventTemplate),
                typeof(DataTemplate),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnTemplateChanged);

        /// <summary>
        /// DataTemplate for rendering day of week headers.
        /// BindingContext: string (day name)
        /// </summary>
        public static readonly BindableProperty DayOfWeekTemplateProperty =
            BindableProperty.Create(
                nameof(DayOfWeekTemplate),
                typeof(DataTemplate),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnTemplateChanged);

        /// <summary>
        /// DataTemplate selector for dynamic day template selection.
        /// </summary>
        public static readonly BindableProperty DayTemplateSelectorProperty =
            BindableProperty.Create(
                nameof(DayTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(ProCalendarView),
                null,
                propertyChanged: OnTemplateChanged);

        #endregion

        #region Template Properties

        /// <summary>
        /// Gets or sets the DataTemplate for rendering day cells.
        /// </summary>
        public DataTemplate? DayTemplate
        {
            get => (DataTemplate?)GetValue(DayTemplateProperty);
            set => SetValue(DayTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the DataTemplate for rendering the header.
        /// </summary>
        public DataTemplate? HeaderTemplate
        {
            get => (DataTemplate?)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the DataTemplate for rendering events.
        /// </summary>
        public DataTemplate? EventTemplate
        {
            get => (DataTemplate?)GetValue(EventTemplateProperty);
            set => SetValue(EventTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the DataTemplate for rendering day of week headers.
        /// </summary>
        public DataTemplate? DayOfWeekTemplate
        {
            get => (DataTemplate?)GetValue(DayOfWeekTemplateProperty);
            set => SetValue(DayOfWeekTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the DataTemplateSelector for dynamic day template selection.
        /// </summary>
        public DataTemplateSelector? DayTemplateSelector
        {
            get => (DataTemplateSelector?)GetValue(DayTemplateSelectorProperty);
            set => SetValue(DayTemplateSelectorProperty, value);
        }

        #endregion

        #region Template Methods

        /// <summary>
        /// Called when the control template is applied.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get template parts
            _mainGrid = GetTemplateChild(PART_MainGrid) as Grid;
            _headerGrid = GetTemplateChild(PART_HeaderGrid) as Grid;
            _daysGrid = GetTemplateChild(PART_DaysGrid) as Grid;
            _scrollView = GetTemplateChild(PART_ScrollView) as ScrollView;

            // Render calendar with new template
            if (_mainGrid != null || _daysGrid != null)
            {
                RenderCalendar();
            }
        }

        /// <summary>
        /// Gets the appropriate DataTemplate for a day cell.
        /// </summary>
        private DataTemplate? GetDayTemplate(Core.Models.CalendarDay day)
        {
            if (DayTemplateSelector != null)
            {
                return DayTemplateSelector.SelectTemplate(day, this);
            }

            return DayTemplate;
        }

        /// <summary>
        /// Creates a view from a DataTemplate with proper binding context.
        /// </summary>
        private View? CreateViewFromTemplate(DataTemplate? template, object? bindingContext)
        {
            if (template == null)
                return null;

            var content = template.CreateContent();
            
            if (content is View view)
            {
                view.BindingContext = bindingContext;
                return view;
            }
            else if (content is ViewCell viewCell)
            {
                viewCell.BindingContext = bindingContext;
                return viewCell.View;
            }

            return null;
        }

        #endregion
    }
}
