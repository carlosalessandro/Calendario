using ProCalendar.Maui.Controls;

namespace ProCalendar.Maui.Behaviors
{
    /// <summary>
    /// Enables swipe gestures for navigating between months/weeks.
    /// </summary>
    public class SwipeNavigationBehavior : Behavior<ProCalendarView>
    {
        private ProCalendarView? _calendar;
        private double _startX;

        protected override void OnAttachedTo(ProCalendarView bindable)
        {
            base.OnAttachedTo(bindable);
            _calendar = bindable;

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            _calendar.GestureRecognizers.Add(panGesture);
        }

        protected override void OnDetachingFrom(ProCalendarView bindable)
        {
            base.OnDetachingFrom(bindable);
            
            if (_calendar != null)
            {
                var panGesture = _calendar.GestureRecognizers.OfType<PanGestureRecognizer>().FirstOrDefault();
                if (panGesture != null)
                {
                    panGesture.PanUpdated -= OnPanUpdated;
                    _calendar.GestureRecognizers.Remove(panGesture);
                }
            }

            _calendar = null;
        }

        private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startX = e.TotalX;
                    break;

                case GestureStatus.Running:
                    // Optional: Add visual feedback during swipe
                    break;

                case GestureStatus.Completed:
                    var deltaX = e.TotalX - _startX;
                    
                    if (_calendar != null && Math.Abs(deltaX) > 50) // Minimum swipe distance
                    {
                        if (deltaX > 0)
                        {
                            // Swipe right - go to previous
                            _calendar.PreviousCommand?.Execute(null);
                        }
                        else
                        {
                            // Swipe left - go to next
                            _calendar.NextCommand?.Execute(null);
                        }
                    }
                    break;
            }
        }
    }
}
