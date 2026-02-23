namespace ProCalendar.Maui.Core.Enums
{
    /// <summary>
    /// Defines the recurrence pattern for calendar events.
    /// </summary>
    public enum RecurrenceType
    {
        /// <summary>
        /// Event does not recur.
        /// </summary>
        None,

        /// <summary>
        /// Event recurs daily.
        /// </summary>
        Daily,

        /// <summary>
        /// Event recurs weekly.
        /// </summary>
        Weekly,

        /// <summary>
        /// Event recurs monthly.
        /// </summary>
        Monthly,

        /// <summary>
        /// Event recurs yearly.
        /// </summary>
        Yearly,

        /// <summary>
        /// Custom recurrence pattern.
        /// </summary>
        Custom
    }
}
