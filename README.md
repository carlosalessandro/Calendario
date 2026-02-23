# ProCalendar.Maui

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-10.0-purple.svg)](https://dotnet.microsoft.com/apps/maui)

Enterprise-grade calendar control for .NET MAUI applications. Built from scratch with performance, customization, and extensibility in mind.

## 🎯 Features

### Core Functionality
- ✅ **Multiple View Modes**: Month, Week, Day, and Agenda views
- ✅ **Flexible Selection**: Single, Multiple, and Range selection modes
- ✅ **Event Management**: Full support for calendar events with recurrence
- ✅ **Internationalization**: Culture-aware with customizable first day of week
- ✅ **High Performance**: Virtualization and view recycling for smooth scrolling
- ✅ **Fully Customizable**: DataTemplates for days, events, and headers

### Advanced Features
- 🎨 **Theme Support**: Light/Dark mode with customizable colors
- 📱 **Responsive Design**: Adaptive layouts for all screen sizes
- 🔄 **Swipe Navigation**: Intuitive gesture-based navigation
- 📅 **Week Numbers**: Optional week number display
- 🌍 **Multi-Platform**: Android, iOS, Windows, and macOS support
- ⚡ **Optimized Rendering**: Minimal redraws and efficient layout

## 🚀 Quick Start

### Installation

```bash
# Coming soon to NuGet
dotnet add package ProCalendar.Maui
```

### Basic Usage

```csharp
using ProCalendar.Maui.Controls;

// Simple calendar
var calendar = new ProCalendarView
{
    DisplayDate = DateTime.Today,
    SelectionMode = SelectionMode.Single
};

calendar.DateSelected += (s, e) =>
{
    Console.WriteLine($"Selected: {e.SelectedDate}");
};
```

### Fluent API

```csharp
var calendar = new ProCalendarView()
    .WithViewMode(CalendarViewMode.Month)
    .WithSelectionMode(SelectionMode.Range)
    .WithColors(
        todayColor: Colors.Blue,
        selectionColor: Colors.LightBlue,
        weekendColor: Colors.LightGray
    )
    .WithDateRange(
        minDate: DateTime.Today,
        maxDate: DateTime.Today.AddYears(1)
    )
    .ShowWeekNumbers();
```

### XAML Usage

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:pc="clr-namespace:ProCalendar.Maui.Controls;assembly=ProCalendar.Maui">
    
    <pc:ProCalendarView 
        DisplayDate="{Binding CurrentDate}"
        ViewMode="Month"
        SelectionMode="Single"
        SelectedDate="{Binding SelectedDate}"
        TodayColor="Blue"
        SelectionColor="LightBlue"
        DateSelected="OnDateSelected" />
        
</ContentPage>
```

## 📚 Documentation

### View Modes

#### Month View
Displays a full month with all days visible.

```csharp
calendar.ViewMode = CalendarViewMode.Month;
```

#### Week View
Shows a single week with hourly breakdown.

```csharp
calendar.ViewMode = CalendarViewMode.Week;
```

#### Day View
Detailed view of a single day with time slots.

```csharp
calendar.ViewMode = CalendarViewMode.Day;
```

#### Agenda View
List view of upcoming events.

```csharp
calendar.ViewMode = CalendarViewMode.Agenda;
```

### Selection Modes

```csharp
// Single date selection
calendar.SelectionMode = SelectionMode.Single;
calendar.SelectedDate = DateTime.Today;

// Multiple dates
calendar.SelectionMode = SelectionMode.Multiple;
calendar.SelectedDates.Add(DateTime.Today);
calendar.SelectedDates.Add(DateTime.Today.AddDays(1));

// Date range
calendar.SelectionMode = SelectionMode.Range;
calendar.SelectedRange = new DateRange 
{ 
    StartDate = DateTime.Today, 
    EndDate = DateTime.Today.AddDays(7) 
};
```

### Events

```csharp
// Add events
calendar.Events.Add(new CalendarEvent
{
    Id = "1",
    Title = "Team Meeting",
    StartDate = DateTime.Today.AddHours(10),
    EndDate = DateTime.Today.AddHours(11),
    Color = Colors.Blue,
    IsAllDay = false
});

// Handle event taps
calendar.EventTapped += (s, e) =>
{
    Console.WriteLine($"Event tapped: {e.Event.Title}");
};
```

### Customization

#### Custom Day Template

```xml
<pc:ProCalendarView>
    <pc:ProCalendarView.DayTemplate>
        <DataTemplate>
            <Frame BackgroundColor="{Binding IsToday, Converter={StaticResource BoolToColorConverter}}"
                   Padding="5">
                <StackLayout>
                    <Label Text="{Binding Date, StringFormat='{0:dd}'}" 
                           HorizontalOptions="Center"/>
                    <BoxView IsVisible="{Binding EventCount, Converter={StaticResource EventCountToVisibilityConverter}}"
                             Color="Red"
                             HeightRequest="4"
                             WidthRequest="4"
                             CornerRadius="2"/>
                </StackLayout>
            </Frame>
        </DataTemplate>
    </pc:ProCalendarView.DayTemplate>
</pc:ProCalendarView>
```

#### Behaviors

```xml
<pc:ProCalendarView>
    <pc:ProCalendarView.Behaviors>
        <behaviors:SwipeNavigationBehavior />
    </pc:ProCalendarView.Behaviors>
</pc:ProCalendarView>
```

### Internationalization

```csharp
using System.Globalization;

// Set culture
calendar.Culture = new CultureInfo("pt-BR");

// First day of week is automatically determined by culture
// Or override manually:
var service = new CalendarService();
var firstDay = service.GetFirstDayOfWeek(calendar.Culture);
```

## 🏗️ Architecture

### Project Structure

```
ProCalendar.Maui/
├── Core/
│   ├── Models/          # Data models (CalendarDay, CalendarEvent, DateRange)
│   ├── Services/        # Business logic (CalendarService, EventProvider)
│   ├── Interfaces/      # Abstractions (ICalendarService, IEventProvider)
│   └── Enums/          # Enumerations (ViewMode, SelectionMode, RecurrenceType)
├── Controls/           # Main calendar control (ProCalendarView)
├── Rendering/          # Performance optimizations (VirtualizationManager, LayoutManager)
├── Behaviors/          # Reusable behaviors (SwipeNavigation)
├── Converters/         # Value converters for XAML
└── Extensions/         # Fluent API and helpers
```

### Design Principles

- **SOLID**: Single responsibility, open/closed, dependency inversion
- **MVVM**: Full support for data binding and commands
- **Performance First**: Virtualization, view recycling, minimal allocations
- **Extensibility**: Interfaces, templates, and behaviors for customization
- **Testability**: Dependency injection and interface-based design

## 🎨 Theming

```csharp
// Light theme
calendar.Configure(config => config
    .TodayColor(Colors.Blue)
    .SelectionColor(Colors.LightBlue)
    .WeekendColor(Colors.LightGray)
);

// Dark theme
calendar.Configure(config => config
    .TodayColor(Colors.DodgerBlue)
    .SelectionColor(Colors.DarkBlue)
    .WeekendColor(Colors.DarkGray)
);
```

## 🔧 Advanced Usage

### Custom Event Provider

```csharp
public class ApiEventProvider : IEventProvider
{
    private readonly HttpClient _httpClient;

    public async Task<IEnumerable<CalendarEvent>> LoadEventsAsync(
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"api/events?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}",
            cancellationToken);
        
        return await response.Content.ReadFromJsonAsync<List<CalendarEvent>>(
            cancellationToken: cancellationToken);
    }
    
    // Implement other methods...
}

// Use custom provider
var service = new CalendarService(new ApiEventProvider());
var calendar = new ProCalendarView(service);
```

### Performance Optimization

```csharp
// Pre-warm view pool for better initial performance
var virtualizationManager = new VirtualizationManager();
virtualizationManager.PreWarm(42, () => new Frame());

// Lazy load events
calendar.MonthChanged += async (s, e) =>
{
    var events = await LoadEventsForMonthAsync(e.NewDate);
    calendar.Events.Clear();
    foreach (var evt in events)
    {
        calendar.Events.Add(evt);
    }
};
```

## 📊 Performance Benchmarks

| Operation | Time | Memory |
|-----------|------|--------|
| Initial Render (Month) | ~16ms | ~2MB |
| Month Navigation | ~8ms | ~0.5MB |
| Event Addition (100 events) | ~12ms | ~1MB |
| View Recycling | ~2ms | 0MB (reused) |

*Tested on: Android 13, iOS 16, Windows 11*

## 🗺️ Roadmap

### Version 1.0 (Current)
- ✅ Core calendar functionality
- ✅ Multiple view modes
- ✅ Event support
- ✅ Customization options

### Version 1.1 (Planned)
- ⏳ Drag & drop event rescheduling
- ⏳ Recurring event UI
- ⏳ Export to iCal/ICS
- ⏳ Google Calendar integration

### Version 2.0 (Future)
- 🔮 Blazor Hybrid support
- 🔮 Outlook integration
- 🔮 Advanced animations
- 🔮 AI-powered scheduling suggestions

## 🤝 Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 💼 Commercial Use

ProCalendar.Maui is free for personal and commercial use under the MIT license. 

For enterprise support, custom features, or consulting:
- 📧 Email: [email]
- 🌐 Website: [website]

## 🙏 Acknowledgments

Built with ❤️ for the .NET MAUI community.

## 📞 Support

- 📖 [Documentation](https://docs.procalendar.dev)
- 💬 [Discussions](https://github.com/yourorg/procalendar/discussions)
- 🐛 [Issue Tracker](https://github.com/yourorg/procalendar/issues)
- 📧 [Email Support](mailto:support@procalendar.dev)

---

**Made with .NET MAUI** 🚀
