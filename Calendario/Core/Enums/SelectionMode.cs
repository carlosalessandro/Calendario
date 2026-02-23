namespace ProCalendar.Maui.Core.Enums
{
    /// <summary>
    /// Defines how dates can be selected in the calendar.
    /// </summary>
    public enum CalendarSelectionMode
    {
        /// <summary>
        /// No date selection allowed.
        /// </summary>
        None,

        /// <summary>
        /// Only one date can be selected at a time.
        /// </summary>
        Single,

        /// <summary>
        /// Multiple individual dates can be selected.
        /// </summary>
        Multiple,

        /// <summary>
        /// A continuous range of dates can be selected.
        /// </summary>
        Range
    }
}
