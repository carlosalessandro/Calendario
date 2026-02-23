namespace ProCalendar.Maui.Core.Models
{
    /// <summary>
    /// Represents a range of dates with start and end points.
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Start date of the range.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the range.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets the total number of days in the range.
        /// </summary>
        public int TotalDays => (EndDate - StartDate).Days + 1;

        /// <summary>
        /// Checks if a date falls within this range.
        /// </summary>
        public bool Contains(DateTime date)
        {
            return date.Date >= StartDate.Date && date.Date <= EndDate.Date;
        }

        /// <summary>
        /// Checks if this range overlaps with another range.
        /// </summary>
        public bool Overlaps(DateRange other)
        {
            return StartDate <= other.EndDate && EndDate >= other.StartDate;
        }
    }
}
