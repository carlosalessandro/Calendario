# ProCalendar.Maui - Usage Examples

## 📚 Table of Contents

1. [Basic Usage](#basic-usage)
2. [Selection Modes](#selection-modes)
3. [Event Management](#event-management)
4. [Customization](#customization)
5. [Advanced Scenarios](#advanced-scenarios)
6. [MVVM Integration](#mvvm-integration)

---

## Basic Usage

### Simple Calendar

```csharp
using ProCalendar.Maui.Controls;

public class MainPage : ContentPage
{
    public MainPage()
    {
        var calendar = new ProCalendarView
        {
            DisplayDate = DateTime.Today,
            SelectionMode = SelectionMode.Single
        };

        calendar.DateSelected += (s, e) =>
        {
            DisplayAlert("Date Selected", e.SelectedDate.ToString("D"), "OK");
        };

        Content = calendar;
    }
}
```

### XAML Usage

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:pc="clr-namespace:ProCalendar.Maui.Controls;assembly=Calendario"
             x:Class="MyApp.MainPage">
    
    <pc:ProCalendarView 
        x:Name="calendar"
        DisplayDate="{Binding CurrentDate}"
        SelectionMode="Single"
        DateSelected="OnDateSelected" />
        
</ContentPage>
```

```csharp
private void OnDateSelected(object sender, DateSelectedEventArgs e)
{
    DisplayAlert("Selected", e.SelectedDate.ToString("D"), "OK");
}
```

---

## Selection Modes

### Single Selection

```csharp
var calendar = new ProCalendarView
{
    SelectionMode = SelectionMode.Single,
    SelectedDate = DateTime.Today
};

calendar.DateSelected += (s, e) =>
{
    Console.WriteLine($"Selected: {e.SelectedDate:d}");
};
```

### Multiple Selection

```csharp
var calendar = new ProCalendarView
{
    SelectionMode = SelectionMode.Multiple
};

// Pre-select dates
calendar.SelectedDates.Add(DateTime.Today);
calendar.SelectedDates.Add(DateTime.Today.AddDays(1));
calendar.SelectedDates.Add(DateTime.Today.AddDays(7));

// Monitor changes
calendar.SelectedDates.CollectionChanged += (s, e) =>
{
    Console.WriteLine($"Total selected: {calendar.SelectedDates.Count}");
};
```

### Range Selection

```csharp
var calendar = new ProCalendarView
{
    SelectionMode = SelectionMode.Range
};

calendar.RangeSelected += (s, e) =>
{
    var range = e.SelectedRange;
    Console.WriteLine($"Range: {range.StartDate:d} to {range.EndDate:d}");
    Console.WriteLine($"Total days: {range.TotalDays}");
};
```

---

## Event Management

### Adding Events

```csharp
using ProCalendar.Maui.Core.Models;
using ProCalendar.Maui.Core.Enums;

// Single event
calendar.Events.Add(new CalendarEvent
{
    Id = "1",
    Title = "Team Meeting",
    Description = "Weekly sync with the team",
    StartDate = DateTime.Today.AddHours(10),
    EndDate = DateTime.Today.AddHours(11),
    Color = Colors.Blue,
    IsAllDay = false
});

// All-day event
calendar.Events.Add(new CalendarEvent
{
    Id = "2",
    Title = "Company Holiday",
    StartDate = DateTime.Today.AddDays(7),
    EndDate = DateTime.Today.AddDays(7),
    Color = Colors.Green,
    IsAllDay = true
});

// Recurring event
calendar.Events.Add(new CalendarEvent
{
    Id = "3",
    Title = "Daily Standup",
    StartDate = DateTime.Today.AddHours(9),
    EndDate = DateTime.Today.AddHours(9).AddMinutes(15),
    Color = Colors.Orange,
    RecurrenceType = RecurrenceType.Daily
});
```

### Loading Events from API

```csharp
public async Task LoadEventsAsync()
{
    var httpClient = new HttpClient();
    var response = await httpClient.GetAsync("https://api.example.com/events");
    var events = await response.Content.ReadFromJsonAsync<List<CalendarEvent>>();

    calendar.Events.Clear();
    foreach (var evt in events)
    {
        calendar.Events.Add(evt);
    }
}
```

### Event Provider Pattern

```csharp
using ProCalendar.Maui.Core.Interfaces;
using ProCalendar.Maui.Core.Services;

public class ApiEventProvider : IEventProvider
{
    private readonly HttpClient _httpClient;

    public ApiEventProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CalendarEvent>> LoadEventsAsync(
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        var url = $"api/events?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        return await response.Content.ReadFromJsonAsync<List<CalendarEvent>>(
            cancellationToken: cancellationToken);
    }

    public async Task<CalendarEvent> AddEventAsync(CalendarEvent calendarEvent, 
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/events", calendarEvent, cancellationToken);
        return await response.Content.ReadFromJsonAsync<CalendarEvent>(cancellationToken: cancellationToken);
    }

    // Implement other methods...
}

// Usage
var provider = new ApiEventProvider(new HttpClient());
var service = new CalendarService(provider);
var calendar = new ProCalendarView(service);
```

### Lazy Loading Events

```csharp
calendar.MonthChanged += async (s, e) =>
{
    var startDate = new DateTime(e.NewDate.Year, e.NewDate.Month, 1);
    var endDate = startDate.AddMonths(1).AddDays(-1);

    var events = await LoadEventsForMonthAsync(startDate, endDate);
    
    calendar.Events.Clear();
    foreach (var evt in events)
    {
        calendar.Events.Add(evt);
    }
};
```

---

## Customization

### Colors

```csharp
var calendar = new ProCalendarView
{
    TodayColor = Colors.Blue,
    SelectionColor = Colors.LightBlue,
    WeekendColor = Colors.LightGray,
    HeaderBackgroundColor = Colors.DarkBlue
};
```

### Custom Day Template

```xml
<pc:ProCalendarView>
    <pc:ProCalendarView.DayTemplate>
        <DataTemplate>
            <Frame BackgroundColor="{Binding IsSelected, Converter={StaticResource BoolToColorConverter}}"
                   BorderColor="{Binding IsToday, Converter={StaticResource BoolToColorConverter}}"
                   Padding="8"
                   Margin="2"
                   CornerRadius="8">
                <Grid>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="Auto"/>
                    </Grid.RowDefinitions>
                    
                    <!-- Day number -->
                    <Label Grid.Row="0"
                           Text="{Binding Date, StringFormat='{0:dd}'}"
                           FontSize="16"
                           FontAttributes="{Binding IsToday, Converter={StaticResource BoolToFontAttributesConverter}}"
                           HorizontalOptions="Center"
                           TextColor="{Binding IsCurrentMonth, Converter={StaticResource BoolToTextColorConverter}}"/>
                    
                    <!-- Event indicator -->
                    <BoxView Grid.Row="1"
                             IsVisible="{Binding EventCount, Converter={StaticResource EventCountToVisibilityConverter}}"
                             Color="Red"
                             HeightRequest="4"
                             WidthRequest="4"
                             CornerRadius="2"
                             HorizontalOptions="Center"
                             Margin="0,4,0,0"/>
                </Grid>
            </Frame>
        </DataTemplate>
    </pc:ProCalendarView.DayTemplate>
</pc:ProCalendarView>
```

### Custom Event Template

```xml
<pc:ProCalendarView>
    <pc:ProCalendarView.EventTemplate>
        <DataTemplate>
            <Frame BackgroundColor="{Binding Color}"
                   Padding="4"
                   Margin="2"
                   CornerRadius="4"
                   HasShadow="False">
                <StackLayout Spacing="2">
                    <Label Text="{Binding Title}"
                           FontSize="12"
                           FontAttributes="Bold"
                           TextColor="White"/>
                    <Label Text="{Binding StartDate, StringFormat='{0:HH:mm}'}"
                           FontSize="10"
                           TextColor="White"
                           IsVisible="{Binding IsAllDay, Converter={StaticResource InverseBoolConverter}}"/>
                </StackLayout>
            </Frame>
        </DataTemplate>
    </pc:ProCalendarView.EventTemplate>
</pc:ProCalendarView>
```

### Fluent API

```csharp
using ProCalendar.Maui.Extensions;

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

### Configuration Builder

```csharp
var calendar = new ProCalendarView();

calendar.Configure(config => config
    .ViewMode(CalendarViewMode.Month)
    .SelectionMode(SelectionMode.Single)
    .TodayColor(Colors.Blue)
    .SelectionColor(Colors.LightBlue)
    .Culture(new CultureInfo("pt-BR"))
);
```

---

## Advanced Scenarios

### Swipe Navigation

```xml
<pc:ProCalendarView>
    <pc:ProCalendarView.Behaviors>
        <behaviors:SwipeNavigationBehavior />
    </pc:ProCalendarView.Behaviors>
</pc:ProCalendarView>
```

### Week Numbers

```csharp
var calendar = new ProCalendarView
{
    ShowWeekNumbers = true
};
```

### Date Restrictions

```csharp
var calendar = new ProCalendarView
{
    MinimumDate = DateTime.Today,
    MaximumDate = DateTime.Today.AddMonths(6)
};
```

### Internationalization

```csharp
using System.Globalization;

// Portuguese (Brazil)
calendar.Culture = new CultureInfo("pt-BR");

// Spanish (Spain)
calendar.Culture = new CultureInfo("es-ES");

// Japanese
calendar.Culture = new CultureInfo("ja-JP");

// Arabic (Saudi Arabia) - RTL
calendar.Culture = new CultureInfo("ar-SA");
```

### Multiple View Modes

```csharp
// Month view
calendar.ViewMode = CalendarViewMode.Month;

// Week view
calendar.ViewMode = CalendarViewMode.Week;

// Day view
calendar.ViewMode = CalendarViewMode.Day;

// Agenda view
calendar.ViewMode = CalendarViewMode.Agenda;
```

### Navigation Commands

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>
    
    <!-- Navigation buttons -->
    <Grid Grid.Row="0" ColumnDefinitions="*,*,*">
        <Button Grid.Column="0" 
                Text="Previous" 
                Command="{Binding Source={x:Reference calendar}, Path=PreviousCommand}"/>
        <Button Grid.Column="1" 
                Text="Today" 
                Command="{Binding Source={x:Reference calendar}, Path=TodayCommand}"/>
        <Button Grid.Column="2" 
                Text="Next" 
                Command="{Binding Source={x:Reference calendar}, Path=NextCommand}"/>
    </Grid>
    
    <pc:ProCalendarView x:Name="calendar" Grid.Row="1"/>
</Grid>
```

---

## MVVM Integration

### ViewModel

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProCalendar.Maui.Core.Models;
using System.Collections.ObjectModel;

public partial class CalendarViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime currentDate = DateTime.Today;

    [ObservableProperty]
    private DateTime? selectedDate;

    [ObservableProperty]
    private ObservableCollection<CalendarEvent> events = new();

    [RelayCommand]
    private async Task LoadEventsAsync()
    {
        // Load events from API
        var loadedEvents = await _eventService.GetEventsAsync(CurrentDate);
        
        Events.Clear();
        foreach (var evt in loadedEvents)
        {
            Events.Add(evt);
        }
    }

    [RelayCommand]
    private async Task AddEventAsync(CalendarEvent newEvent)
    {
        await _eventService.AddEventAsync(newEvent);
        Events.Add(newEvent);
    }

    [RelayCommand]
    private void OnDateSelected(DateTime date)
    {
        SelectedDate = date;
        // Additional logic
    }
}
```

### View (XAML)

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:pc="clr-namespace:ProCalendar.Maui.Controls;assembly=Calendario"
             xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:Class="MyApp.Views.CalendarPage">
    
    <ContentPage.BindingContext>
        <vm:CalendarViewModel />
    </ContentPage.BindingContext>
    
    <Grid RowDefinitions="Auto,*,Auto">
        <!-- Header -->
        <Label Grid.Row="0" 
               Text="{Binding CurrentDate, StringFormat='Current: {0:MMMM yyyy}'}"
               FontSize="20"
               HorizontalOptions="Center"
               Margin="10"/>
        
        <!-- Calendar -->
        <pc:ProCalendarView Grid.Row="1"
                           DisplayDate="{Binding CurrentDate}"
                           SelectedDate="{Binding SelectedDate}"
                           Events="{Binding Events}"
                           SelectionMode="Single"/>
        
        <!-- Actions -->
        <Button Grid.Row="2"
                Text="Load Events"
                Command="{Binding LoadEventsCommand}"
                Margin="10"/>
    </Grid>
    
</ContentPage>
```

### Dependency Injection

```csharp
// MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Register services
        builder.Services.AddSingleton<IEventProvider, ApiEventProvider>();
        builder.Services.AddSingleton<ICalendarService, CalendarService>();
        
        // Register ViewModels
        builder.Services.AddTransient<CalendarViewModel>();
        
        // Register Pages
        builder.Services.AddTransient<CalendarPage>();

        return builder.Build();
    }
}
```

---

## 🎯 Best Practices

1. **Use MVVM**: Bind to ViewModels for better testability
2. **Lazy Load Events**: Load events per month for performance
3. **Dispose Properly**: Unsubscribe from events to prevent leaks
4. **Use Fluent API**: Cleaner, more readable configuration
5. **Custom Templates**: Create branded, unique calendar experiences
6. **Test Thoroughly**: Unit test ViewModels and integration test UI

---

## 📞 Need Help?

- 📖 [Full Documentation](https://docs.procalendar.dev)
- 💬 [GitHub Discussions](https://github.com/yourorg/procalendar/discussions)
- 🐛 [Report Issues](https://github.com/yourorg/procalendar/issues)

---

**Happy Coding!** 🚀
