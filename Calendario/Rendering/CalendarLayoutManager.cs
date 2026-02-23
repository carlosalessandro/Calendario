using ProCalendar.Maui.Core.Enums;
using ProCalendar.Maui.Core.Models;

namespace ProCalendar.Maui.Rendering
{
    /// <summary>
    /// Manages layout calculations and optimizations for calendar rendering.
    /// Handles responsive sizing and adaptive layouts.
    /// </summary>
    public class CalendarLayoutManager
    {
        private const double MinCellSize = 40;
        private const double MaxCellSize = 100;
        private const double DefaultCellSize = 60;

        /// <summary>
        /// Calculates optimal cell size based on available space.
        /// </summary>
        public double CalculateCellSize(double availableWidth, int columns, double spacing)
        {
            var totalSpacing = spacing * (columns - 1);
            var cellSize = (availableWidth - totalSpacing) / columns;
            
            return Math.Clamp(cellSize, MinCellSize, MaxCellSize);
        }

        /// <summary>
        /// Determines the number of rows needed for a month view.
        /// </summary>
        public int CalculateRowsForMonth(int year, int month, DayOfWeek firstDayOfWeek)
        {
            var firstDay = new DateTime(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            
            var startOffset = ((int)firstDay.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
            var totalDays = lastDay.Day + startOffset;
            
            return (int)Math.Ceiling(totalDays / 7.0);
        }

        /// <summary>
        /// Calculates layout metrics for the calendar.
        /// </summary>
        public LayoutMetrics CalculateLayoutMetrics(
            CalendarViewMode viewMode,
            double availableWidth,
            double availableHeight,
            bool showWeekNumbers)
        {
            var metrics = new LayoutMetrics
            {
                ViewMode = viewMode,
                ShowWeekNumbers = showWeekNumbers
            };

            switch (viewMode)
            {
                case CalendarViewMode.Month:
                    metrics.Columns = showWeekNumbers ? 8 : 7;
                    metrics.Rows = 7; // Header + 6 weeks
                    metrics.CellWidth = CalculateCellSize(availableWidth, metrics.Columns, 1);
                    metrics.CellHeight = DefaultCellSize;
                    break;

                case CalendarViewMode.Week:
                    metrics.Columns = 7;
                    metrics.Rows = 24; // 24 hours
                    metrics.CellWidth = CalculateCellSize(availableWidth, metrics.Columns, 1);
                    metrics.CellHeight = 40;
                    break;

                case CalendarViewMode.Day:
                    metrics.Columns = 1;
                    metrics.Rows = 24;
                    metrics.CellWidth = availableWidth;
                    metrics.CellHeight = 60;
                    break;

                case CalendarViewMode.Agenda:
                    metrics.Columns = 1;
                    metrics.Rows = -1; // Dynamic
                    metrics.CellWidth = availableWidth;
                    metrics.CellHeight = 80;
                    break;
            }

            return metrics;
        }

        /// <summary>
        /// Determines if a view should use compact mode based on available space.
        /// </summary>
        public bool ShouldUseCompactMode(double availableWidth)
        {
            return availableWidth < 400;
        }
    }

    /// <summary>
    /// Contains calculated layout metrics for rendering.
    /// </summary>
    public class LayoutMetrics
    {
        public CalendarViewMode ViewMode { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public double CellWidth { get; set; }
        public double CellHeight { get; set; }
        public bool ShowWeekNumbers { get; set; }
        public bool IsCompactMode { get; set; }
    }
}
