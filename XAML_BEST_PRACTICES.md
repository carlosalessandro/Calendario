# ProCalendar.Maui - XAML Best Practices & Anti-Patterns

## 🎯 Best Practices

### 1. Always Set HeightRequest

✅ **DO:**
```xml
<calendar:ProCalendarView 
    HeightRequest="350"
    ViewMode="Month"/>
```

❌ **DON'T:**
```xml
<!-- Calendar may not render properly without height -->
<calendar:ProCalendarView ViewMode="Month"/>
```

**Why**: TemplatedView needs explicit height for proper layout calculation.

---

### 2. Use TwoWay Binding for Editable Properties

✅ **DO:**
```xml
<calendar:ProCalendarView 
    DisplayDate="{Binding CurrentDate, Mode=TwoWay}"
    SelectedDate="{Binding SelectedDate, Mode=TwoWay}"/>
```

❌ **DON'T:**
```xml
<!-- Changes won't propagate back to ViewModel -->
<calendar:ProCalendarView 
    DisplayDate="{Binding CurrentDate}"
    SelectedDate="{Binding SelectedDate}"/>
```

---

### 3. Prefer Commands Over Event Handlers

✅ **DO (MVVM):**
```xml
<calendar:ProCalendarView 
    DateSelectedCommand="{Binding DateSelectedCommand}"/>
```

```csharp
[RelayCommand]
private void OnDateSelected(DateTime date)
{
    // Testable, MVVM-friendly
}
```

❌ **DON'T (Code-behind):**
```xml
<calendar:ProCalendarView 
    x:Name="calendar"/>
```

```csharp
calendar.DateSelected += (s, e) =>
{
    // Hard to test, breaks MVVM
};
```

---

### 4. Keep Templates Simple

✅ **DO:**
```xml
<DataTemplate>
    <Border Padding="8">
        <Label Text="{Binding Date, StringFormat='{0:dd}'}"/>
    </Border>
</DataTemplate>
```

❌ **DON'T:**
```xml
<DataTemplate>
    <StackLayout>
        <Grid>
            <Frame>
                <StackLayout>
                    <Grid>
                        <!-- Too many nested layouts = poor performance -->
                    </Grid>
                </StackLayout>
            </Frame>
        </Grid>
    </StackLayout>
</DataTemplate>
```

**Why**: Complex templates hurt performance, especially with 42+ day cells.

---

### 5. Lazy Load Events

✅ **DO:**
```csharp
[RelayCommand]
private async Task OnMonthChanged(MonthChangedEventArgs args)
{
    var startDate = new DateTime(args.NewDate.Year, args.NewDate.Month, 1);
    var endDate = startDate.AddMonths(1).AddDays(-1);
    
    var events = await _eventService.GetEventsAsync(startDate, endDate);
    
    Events.Clear();
    foreach (var evt in events)
    {
        Events.Add(evt);
    }
}
```

❌ **DON'T:**
```csharp
// Loading all events upfront
public ObservableCollection<CalendarEvent> Events { get; set; } = 
    await _eventService.GetAllEventsAsync(); // Millions of events!
```

---

### 6. Use Styles for Consistency

✅ **DO:**
```xml
<Application.Resources>
    <Style TargetType="calendar:ProCalendarView">
        <Setter Property="TodayColor" Value="Purple"/>
        <Setter Property="SelectionColor" Value="LightPurple"/>
        <Setter Property="DayFontSize" Value="16"/>
    </Style>
</Application.Resources>

<!-- All calendars inherit style -->
<calendar:ProCalendarView />
```

❌ **DON'T:**
```xml
<!-- Repeating properties everywhere -->
<calendar:ProCalendarView TodayColor="Purple" SelectionColor="LightPurple" DayFontSize="16"/>
<calendar:ProCalendarView TodayColor="Purple" SelectionColor="LightPurple" DayFontSize="16"/>
<calendar:ProCalendarView TodayColor="Purple" SelectionColor="LightPurple" DayFontSize="16"/>
```

---

### 7. Limit Events Per Day

✅ **DO:**
```xml
<calendar:ProCalendarView 
    MaxEventsPerDay="3"
    ShowEvents="True"/>
```

❌ **DON'T:**
```xml
<!-- Showing 50+ events per day cell -->
<calendar:ProCalendarView 
    MaxEventsPerDay="999"
    ShowEvents="True"/>
```

**Why**: Too many events per cell causes layout issues and poor UX.

---

### 8. Use DataTemplateSelector for Complex Scenarios

✅ **DO:**
```csharp
public class DayTemplateSelector : DataTemplateSelector
{
    public DataTemplate NormalDayTemplate { get; set; }
    public DataTemplate WeekendDayTemplate { get; set; }
    public DataTemplate TodayTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is CalendarDay day)
        {
            if (day.IsToday) return TodayTemplate;
            if (day.IsWeekend) return WeekendDayTemplate;
            return NormalDayTemplate;
        }
        return null;
    }
}
```

❌ **DON'T:**
```xml
<!-- Complex conditional logic in XAML -->
<DataTemplate>
    <Grid>
        <Border IsVisible="{Binding IsToday}">...</Border>
        <Border IsVisible="{Binding IsWeekend}">...</Border>
        <Border IsVisible="{Binding IsNormal}">...</Border>
    </Grid>
</DataTemplate>
```

---

### 9. Dispose Properly

✅ **DO:**
```csharp
public class CalendarPage : ContentPage
{
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Unsubscribe from events
        if (BindingContext is CalendarViewModel vm)
        {
            vm.Cleanup();
        }
    }
}
```

❌ **DON'T:**
```csharp
// Never cleaning up subscriptions = memory leaks
```

---

### 10. Test on All Platforms

✅ **DO:**
- Test on Android (multiple API levels)
- Test on iOS (iPhone and iPad)
- Test on Windows
- Test on macOS

❌ **DON'T:**
- Only test on one platform
- Assume it works everywhere

---

## 🚫 Anti-Patterns

### 1. Binding to Non-Observable Properties

❌ **WRONG:**
```csharp
public class BadViewModel
{
    public DateTime SelectedDate { get; set; } // Not observable!
}
```

✅ **CORRECT:**
```csharp
public partial class GoodViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime selectedDate; // Observable!
}
```

---

### 2. Modifying UI from Background Thread

❌ **WRONG:**
```csharp
Task.Run(() =>
{
    Events.Add(newEvent); // Crash! UI thread violation
});
```

✅ **CORRECT:**
```csharp
await MainThread.InvokeOnMainThreadAsync(() =>
{
    Events.Add(newEvent);
});
```

---

### 3. Creating New Collections Instead of Clearing

❌ **WRONG:**
```csharp
Events = new ObservableCollection<CalendarEvent>(newEvents); // Breaks binding!
```

✅ **CORRECT:**
```csharp
Events.Clear();
foreach (var evt in newEvents)
{
    Events.Add(evt);
}
```

---

### 4. Not Handling Null Values

❌ **WRONG:**
```xml
<Label Text="{Binding SelectedDate.ToString()}"/> <!-- Crash if null! -->
```

✅ **CORRECT:**
```xml
<Label Text="{Binding SelectedDate, StringFormat='{0:d}', TargetNullValue='No date selected'}"/>
```

---

### 5. Hardcoding Strings

❌ **WRONG:**
```xml
<Label Text="Selected Date:"/>
```

✅ **CORRECT:**
```xml
<Label Text="{x:Static resources:Strings.SelectedDate}"/>
```

---

### 6. Ignoring Culture

❌ **WRONG:**
```csharp
date.ToString("MM/dd/yyyy") // US-only format
```

✅ **CORRECT:**
```csharp
date.ToString("d", Culture) // Culture-aware
```

---

### 7. Not Validating Date Ranges

❌ **WRONG:**
```csharp
SelectedDate = userInput; // Could be invalid!
```

✅ **CORRECT:**
```csharp
if (userInput >= MinimumDate && userInput <= MaximumDate)
{
    SelectedDate = userInput;
}
```

---

### 8. Blocking UI Thread

❌ **WRONG:**
```csharp
var events = _eventService.GetEventsAsync().Result; // Blocks UI!
```

✅ **CORRECT:**
```csharp
var events = await _eventService.GetEventsAsync(); // Async/await
```

---

### 9. Not Using Converters

❌ **WRONG:**
```xml
<Label Text="{Binding IsSelected}"/> <!-- Shows "True" or "False" -->
```

✅ **CORRECT:**
```xml
<Label Text="{Binding IsSelected, Converter={StaticResource BoolToTextConverter}}"/>
```

---

### 10. Overusing Code-Behind

❌ **WRONG:**
```csharp
// CalendarPage.xaml.cs
public partial class CalendarPage : ContentPage
{
    public CalendarPage()
    {
        InitializeComponent();
        
        calendar.DateSelected += OnDateSelected;
        // Lots of logic here...
    }
    
    private void OnDateSelected(object sender, DateSelectedEventArgs e)
    {
        // Business logic in code-behind
    }
}
```

✅ **CORRECT:**
```csharp
// CalendarPage.xaml.cs
public partial class CalendarPage : ContentPage
{
    public CalendarPage()
    {
        InitializeComponent();
        BindingContext = new CalendarViewModel();
    }
}

// CalendarViewModel.cs
public partial class CalendarViewModel : ObservableObject
{
    [RelayCommand]
    private void OnDateSelected(DateTime date)
    {
        // Business logic in ViewModel (testable!)
    }
}
```

---

## 🎨 Template Best Practices

### Use Border Instead of Frame

✅ **DO:**
```xml
<Border BackgroundColor="White" Stroke="Gray" StrokeThickness="1">
    <Label Text="{Binding Date}"/>
</Border>
```

❌ **DON'T:**
```xml
<Frame BorderColor="Gray" BackgroundColor="White">
    <Label Text="{Binding Date}"/>
</Frame>
```

**Why**: Border is lighter and more performant than Frame.

---

### Minimize Bindings

✅ **DO:**
```xml
<Label Text="{Binding Date, StringFormat='{0:dd}'}"/>
```

❌ **DON'T:**
```xml
<Label>
    <Label.Text>
        <MultiBinding StringFormat="{}{0:dd}">
            <Binding Path="Date"/>
        </MultiBinding>
    </Label.Text>
</Label>
```

---

### Use Static Resources

✅ **DO:**
```xml
<ContentPage.Resources>
    <Color x:Key="PrimaryColor">Purple</Color>
</ContentPage.Resources>

<calendar:ProCalendarView TodayColor="{StaticResource PrimaryColor}"/>
```

❌ **DON'T:**
```xml
<calendar:ProCalendarView TodayColor="Purple"/>
<Button BackgroundColor="Purple"/>
<Label TextColor="Purple"/>
<!-- Repeating colors everywhere -->
```

---

## ⚡ Performance Best Practices

### 1. Pre-compile XAML

Ensure in .csproj:
```xml
<MauiXamlInflator>SourceGen</MauiXamlInflator>
```

### 2. Use Compiled Bindings

```xml
<ContentPage xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:DataType="vm:CalendarViewModel">
    
    <calendar:ProCalendarView 
        SelectedDate="{Binding SelectedDate}"/> <!-- Compiled! -->
</ContentPage>
```

### 3. Avoid Unnecessary Layouts

✅ **DO:**
```xml
<Grid ColumnDefinitions="*,*">
    <Label Grid.Column="0"/>
    <Label Grid.Column="1"/>
</Grid>
```

❌ **DON'T:**
```xml
<StackLayout Orientation="Horizontal">
    <Label HorizontalOptions="FillAndExpand"/>
    <Label HorizontalOptions="FillAndExpand"/>
</StackLayout>
```

---

## 🧪 Testing Best Practices

### Unit Test ViewModels

```csharp
[Test]
public void DateSelected_UpdatesSelectedDate()
{
    // Arrange
    var vm = new CalendarViewModel();
    var testDate = new DateTime(2024, 1, 15);
    
    // Act
    vm.OnDateSelectedCommand.Execute(testDate);
    
    // Assert
    Assert.That(vm.SelectedDate, Is.EqualTo(testDate));
}
```

### Integration Test with Calendar

```csharp
[Test]
public async Task Calendar_LoadsEventsOnMonthChange()
{
    // Arrange
    var calendar = new ProCalendarView();
    var vm = new CalendarViewModel();
    calendar.BindingContext = vm;
    
    // Act
    calendar.DisplayDate = new DateTime(2024, 2, 1);
    await Task.Delay(100); // Wait for async load
    
    // Assert
    Assert.That(vm.Events.Count, Is.GreaterThan(0));
}
```

---

## 📚 Documentation Best Practices

### XML Documentation

```csharp
/// <summary>
/// Gets or sets the selected date.
/// </summary>
/// <value>
/// The selected date, or null if no date is selected.
/// </value>
/// <remarks>
/// This property supports TwoWay binding.
/// </remarks>
public DateTime? SelectedDate { get; set; }
```

### Code Comments

```csharp
// ✅ Good: Explains WHY
// Using Border instead of Frame for better performance
var border = new Border();

// ❌ Bad: Explains WHAT (obvious from code)
// Create a new border
var border = new Border();
```

---

## 🎯 Accessibility Best Practices

### Add Semantic Properties

```xml
<calendar:ProCalendarView 
    SemanticProperties.Description="Calendar for selecting dates"
    SemanticProperties.Hint="Swipe left or right to change months"/>
```

### Support Screen Readers

```csharp
SemanticProperties.SetDescription(dayView, $"Day {day.Date:d}");
SemanticProperties.SetHint(dayView, day.IsToday ? "Today" : "");
```

---

## 🔒 Security Best Practices

### Validate User Input

```csharp
public DateTime? SelectedDate
{
    get => _selectedDate;
    set
    {
        if (value.HasValue)
        {
            if (value < MinimumDate || value > MaximumDate)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
        _selectedDate = value;
    }
}
```

### Sanitize Event Data

```csharp
public void AddEvent(CalendarEvent evt)
{
    // Sanitize title
    evt.Title = evt.Title?.Trim() ?? "Untitled";
    
    // Validate dates
    if (evt.EndDate < evt.StartDate)
    {
        throw new ArgumentException("End date must be after start date");
    }
    
    Events.Add(evt);
}
```

---

## 📞 Support

For questions about best practices:
- Review this guide
- Check XAML_USAGE_GUIDE.md
- See Examples/ folder
- Open GitHub Discussion

---

**Follow these practices for production-ready code!** 🚀
