using ProCalendar.Maui.Core.Models;

namespace ProCalendar.Maui.Rendering
{
    /// <summary>
    /// Manages virtualization of calendar cells for optimal performance.
    /// Implements object pooling and lazy loading strategies.
    /// </summary>
    public class VirtualizationManager
    {
        private readonly Queue<View> _viewPool = new();
        private readonly Dictionary<DateTime, View> _activeViews = new();
        private const int MaxPoolSize = 50;

        /// <summary>
        /// Gets or creates a view for the specified day.
        /// </summary>
        public View GetOrCreateView(CalendarDay day, Func<CalendarDay, View> createView)
        {
            // Check if view is already active
            if (_activeViews.TryGetValue(day.Date, out var existingView))
            {
                UpdateView(existingView, day);
                return existingView;
            }

            // Try to reuse from pool
            View view;
            if (_viewPool.Count > 0)
            {
                view = _viewPool.Dequeue();
                UpdateView(view, day);
            }
            else
            {
                view = createView(day);
            }

            _activeViews[day.Date] = view;
            return view;
        }

        /// <summary>
        /// Recycles views that are no longer visible.
        /// </summary>
        public void RecycleViews(IEnumerable<DateTime> visibleDates)
        {
            var datesToRemove = _activeViews.Keys
                .Where(date => !visibleDates.Contains(date))
                .ToList();

            foreach (var date in datesToRemove)
            {
                if (_activeViews.TryGetValue(date, out var view))
                {
                    _activeViews.Remove(date);
                    
                    if (_viewPool.Count < MaxPoolSize)
                    {
                        _viewPool.Enqueue(view);
                    }
                }
            }
        }

        /// <summary>
        /// Updates an existing view with new data.
        /// </summary>
        private void UpdateView(View view, CalendarDay day)
        {
            view.BindingContext = day;
        }

        /// <summary>
        /// Clears all cached views and pool.
        /// </summary>
        public void Clear()
        {
            _activeViews.Clear();
            _viewPool.Clear();
        }

        /// <summary>
        /// Pre-warms the pool with views for better initial performance.
        /// </summary>
        public void PreWarm(int count, Func<View> createView)
        {
            for (int i = 0; i < Math.Min(count, MaxPoolSize); i++)
            {
                _viewPool.Enqueue(createView());
            }
        }
    }
}
