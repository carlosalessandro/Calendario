# ProCalendar.Maui - XAML Usage Guide

## 🎯 Overview

ProCalendar.Maui is fully XAML-ready with complete support for:
- ✅ Data binding (TwoWay where appropriate)
- ✅ DataTemplates for customization
- ✅ ControlTemplate for complete UI override
- ✅ Commands for MVVM
- ✅ Styles and ResourceDictionaries
- ✅ Design-time support

---

## 📦 Setup

### 1. Add Namespace

```xml
xmlns:calendar="clr-namespace:ProCalendar.Maui.Controls;assembly=ProCalendar.Maui"
```

### 2. Include Default Theme (Optional)

In your `App.xaml`:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Themes/DefaultCalendarTheme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

---

## 🚀 Basic Usage

### Simple Calendar

```xml
<calendar:ProCalendarView 
    HeightRequest="350"
    ViewMode="Month"
    SelectionMode="Single"
    TodayColor="Blue"
    SelectionColor="LightBlue"/>
```

### With Data Binding

```xml
<calendar:ProCalendarView 
    DisplayDate="{Binding CurrentDate, Mode=TwoWay}"
    SelectedDate="{Binding SelectedDate, Mode=TwoWay}"
    EventsSource="{Binding Events}"
    DateSelectedCommand="{Binding DateSelectedCommand}"/>
```

---

## 🎨 Customization

### Colors and Appearance

```xml
<calendar:ProCalendarView 
    TodayColor="Purple"
    SelectionColor="LightPurple"
    WeekendColor="LightGray"
    HeaderBackgroundColor="DarkPurple"
    DayTextColor="White"
    DayFontSize="16"/>
```

### Culture and Localization

```xml
<calendar:ProCalendarView 
    Culture="{x:Static globalization:CultureInfo.CurrentCulture}"
    FirstDayOfWeek="Monday"/>
```

### Features

```xml
<calendar:ProCalendarView 
    ShowWeekNumbers="True"
    IsSwipeEnabled="True"
    ShowEvents="True"
    MaxEventsPerDay="5"
    MinimumDate="{Binding MinDate}"
    MaximumDate="{Binding MaxDate}"/>
```

---

## 📋 Selection Modes

### Single Selection

```xml
<calendar:ProCalendarView 
    SelectionMode="Single"
    SelectedDate="{Binding SelectedDate, Mode=TwoWay}"
    DateSelectedCommand="{Binding OnDateSelectedCommand}"/>
```

### Multiple Selection

```xml
<calendar:ProCalendarView 
    SelectionMode="Multiple"
    SelectedDates="{Binding SelectedDates, Mode=TwoWay}"/>
```

### Range Selection

```xml
<calendar:ProCalendarView 
    SelectionMode="Range"
    SelectedRange="{Binding SelectedRange, Mode=TwoWay}"
    RangeSelectedCommand="{Binding OnRangeSelectedCommand}"/>
```

---

## 🎭 DataTemplates

### Custom Day Template

```xml
<calendar:ProCalendarView>
    <calendar:ProCalendarView.DayTemplate>
        <DataTemplate>
            <Border BackgroundColor="{Binding IsSelected, Converter={StaticResource BoolToColorConverter}}"
                    Stroke="{Binding IsToday, Converter={StaticResource BoolToColorConverter}}"
                    StrokeThickness="2"
                    Padding="8"
                    Margin="2">
                <Border.StrokeShape>
                    <RoundRectangle CornerRadius="8"/>
                </Border.StrokeShape>
                
                <StackLayout>
                    <!-- Day Number -->
                    <Label Text="{Binding Date, StringFormat='{0:dd}'}"
                           FontSize="16"
                           HorizontalOptions="Center"/>
                    
                    <!-- Event Indicator -->
                    <BoxView IsVisible="{Binding EventCount, Converter={StaticResource EventCountToVisibilityConverter}}"
                             Color="Red"
                             HeightRequest="4"
                             WidthRequest="4"
                             CornerRadius="2"
                             HorizontalOptions="Center"/>
                </StackLayout>
            </Border>
        </DataTemplate>
    </calendar:ProCalendarView.DayTemplate>
</calendar:ProCalendarView>
```

### Custom Header Template

```xml
<calendar:ProCalendarView>
    <calendar:ProCalendarView.HeaderTemplate>
        <DataTemplate>
            <Grid ColumnDefinitions="Auto,*,Auto" Padding="10">
                <Button Grid.Column="0"
                        Text="◀"
                        Command="{Binding Source={RelativeSource AncestorType={x:Type calendar:ProCalendarView}}, Path=PreviousCommand}"/>
                
                <Label Grid.Column="1"
                       Text="{Binding Source={RelativeSource AncestorType={x:Type calendar:ProCalendarView}}, Path=DisplayDate, StringFormat='{0:MMMM yyyy}'}"
                       FontSize="20"
                       HorizontalOptions="Center"
                       VerticalOptions="Center"/>
                
                <Button Grid.Column="2"
                        Text="▶"
                        Command="{Binding Source={RelativeSource AncestorType={x:Type calendar:ProCalendarView}}, Path=NextCommand}"/>
            </Grid>
        </DataTemplate>
    </calendar:ProCalendarView.HeaderTemplate>
</calendar:ProCalendarView>
```

### Custom Event Template

```xml
<calendar:ProCalendarView>
    <calendar:ProCalendarView.EventTemplate>
        <DataTemplate>
            <Border BackgroundColor="{Binding Color}"
                    Padding="4"
                    Margin="2">
                <Border.StrokeShape>
                    <RoundRectangle CornerRadius="4"/>
                </Border.StrokeShape>
                
                <StackLayout Spacing="2">
                    <Label Text="{Binding Title}"
                           FontSize="10"
                           FontAttributes="Bold"
                           TextColor="White"
                           LineBreakMode="TailTruncation"/>
                    
                    <Label Text="{Binding StartDate, StringFormat='{0:HH:mm}'}"
                           FontSize="8"
                           TextColor="White"
                           IsVisible="{Binding IsAllDay, Converter={StaticResource InverseBoolConverter}}"/>
                </StackLayout>
            </Border>
        </DataTemplate>
    </calendar:ProCalendarView.EventTemplate>
</calendar:ProCalendarView>
```

---

## 🎨 Styles

### Global Style

```xml
<Application.Resources>
    <Style TargetType="calendar:ProCalendarView">
        <Setter Property="TodayColor" Value="Purple"/>
        <Setter Property="SelectionColor" Value="LightPurple"/>
        <Setter Property="WeekendColor" Value="LightGray"/>
        <Setter Property="DayFontSize" Value="16"/>
        <Setter Property="IsSwipeEnabled" Value="True"/>
    </Style>
</Application.Resources>
```

### Named Style

```xml
<ContentPage.Resources>
    <Style x:Key="CompactCalendarStyle" TargetType="calendar:ProCalendarView">
        <Setter Property="DayFontSize" Value="12"/>
        <Setter Property="ShowWeekNumbers" Value="False"/>
        <Setter Property="ShowEvents" Value="False"/>
    </Style>
    
    <Style x:Key="DetailedCalendarStyle" TargetType="calendar:ProCalendarView">
        <Setter Property="DayFontSize" Value="18"/>
        <Setter Property="ShowWeekNumbers" Value="True"/>
        <Setter Property="ShowEvents" Value="True"/>
        <Setter Property="MaxEventsPerDay" Value="5"/>
    </Style>
</ContentPage.Resources>

<!-- Usage -->
<calendar:ProCalendarView Style="{StaticResource CompactCalendarStyle}"/>
```

---

## 🎯 MVVM Integration

### ViewModel

```csharp
public class CalendarViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime currentDate = DateTime.Today;

    [ObservableProperty]
    private DateTime? selectedDate;

    [ObservableProperty]
    private ObservableCollection<CalendarEvent> events = new();

    [RelayCommand]
    private void OnDateSelected(DateTime date)
    {
        SelectedDate = date;
        // Load events for date
    }

    [RelayCommand]
    private void OnRangeSelected(DateRange range)
    {
        // Handle range selection
    }

    [RelayCommand]
    private async Task OnMonthChanged(MonthChangedEventArgs args)
    {
        // Lazy load events for new month
        await LoadEventsForMonthAsync(args.NewDate);
    }
}
```

### View (XAML)

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:calendar="clr-namespace:ProCalendar.Maui.Controls;assembly=ProCalendar.Maui"
             xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:Class="MyApp.Views.CalendarPage">
    
    <ContentPage.BindingContext>
        <vm:CalendarViewModel />
    </ContentPage.BindingContext>
    
    <Grid RowDefinitions="*,Auto">
        <!-- Calendar -->
        <calendar:ProCalendarView 
            Grid.Row="0"
            DisplayDate="{Binding CurrentDate, Mode=TwoWay}"
            SelectedDate="{Binding SelectedDate, Mode=TwoWay}"
            EventsSource="{Binding Events}"
            DateSelectedCommand="{Binding DateSelectedCommand}"
            MonthChangedCommand="{Binding MonthChangedCommand}"/>
        
        <!-- Status Bar -->
        <Label Grid.Row="1"
               Text="{Binding StatusMessage}"
               Padding="10"
               BackgroundColor="LightGray"/>
    </Grid>
    
</ContentPage>
```

---

## 🔧 ControlTemplate

### Custom Control Template

```xml
<ContentPage.Resources>
    <ControlTemplate x:Key="CustomCalendarTemplate">
        <Grid RowDefinitions="Auto,Auto,*" BackgroundColor="White">
            
            <!-- Custom Header -->
            <Border Grid.Row="0"
                    BackgroundColor="Purple"
                    Padding="15">
                <Grid ColumnDefinitions="Auto,*,Auto,Auto">
                    <ImageButton Grid.Column="0"
                                Source="arrow_left.png"
                                Command="{TemplateBinding PreviousCommand}"/>
                    
                    <Label Grid.Column="1"
                           Text="{TemplateBinding DisplayDate, StringFormat='{0:MMMM yyyy}'}"
                           FontSize="22"
                           FontAttributes="Bold"
                           TextColor="White"
                           HorizontalOptions="Center"
                           VerticalOptions="Center"/>
                    
                    <Button Grid.Column="2"
                            Text="Today"
                            Command="{TemplateBinding TodayCommand}"
                            BackgroundColor="White"
                            TextColor="Purple"
                            Margin="5,0"/>
                    
                    <ImageButton Grid.Column="3"
                                Source="arrow_right.png"
                                Command="{TemplateBinding NextCommand}"/>
                </Grid>
            </Border>
            
            <!-- View Mode Selector -->
            <HorizontalStackLayout Grid.Row="1" Padding="10" Spacing="5">
                <Button Text="Month" Command="{TemplateBinding SwitchToMonthCommand}"/>
                <Button Text="Week" Command="{TemplateBinding SwitchToWeekCommand}"/>
                <Button Text="Day" Command="{TemplateBinding SwitchToDayCommand}"/>
            </HorizontalStackLayout>
            
            <!-- Days Grid (Required Template Part) -->
            <Grid x:Name="PART_DaysGrid"
                  Grid.Row="2"
                  RowSpacing="1"
                  ColumnSpacing="1"
                  BackgroundColor="LightGray"/>
        </Grid>
    </ControlTemplate>
</ContentPage.Resources>

<!-- Usage -->
<calendar:ProCalendarView ControlTemplate="{StaticResource CustomCalendarTemplate}"/>
```

---

## 📊 Events Source Binding

### Binding to ObservableCollection

```xml
<calendar:ProCalendarView 
    EventsSource="{Binding Events}"/>
```

```csharp
public ObservableCollection<CalendarEvent> Events { get; set; } = new();

// Add events
Events.Add(new CalendarEvent
{
    Title = "Meeting",
    StartDate = DateTime.Today.AddHours(10),
    EndDate = DateTime.Today.AddHours(11),
    Color = Colors.Blue
});
```

### Lazy Loading Events

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

---

## 🎨 DataTemplateSelector

### Custom Template Selector

```csharp
public class DayTemplateSelector : DataTemplateSelector
{
    public DataTemplate? NormalDayTemplate { get; set; }
    public DataTemplate? WeekendDayTemplate { get; set; }
    public DataTemplate? TodayTemplate { get; set; }
    public DataTemplate? EventDayTemplate { get; set; }

    protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
    {
        if (item is CalendarDay day)
        {
            if (day.IsToday)
                return TodayTemplate;
            
            if (day.EventCount > 0)
                return EventDayTemplate;
            
            if (day.IsWeekend)
                return WeekendDayTemplate;
            
            return NormalDayTemplate;
        }

        return null;
    }
}
```

### Usage in XAML

```xml
<ContentPage.Resources>
    <local:DayTemplateSelector x:Key="DaySelector">
        <local:DayTemplateSelector.NormalDayTemplate>
            <DataTemplate>
                <!-- Normal day template -->
            </DataTemplate>
        </local:DayTemplateSelector.NormalDayTemplate>
        
        <local:DayTemplateSelector.WeekendDayTemplate>
            <DataTemplate>
                <!-- Weekend template -->
            </DataTemplate>
        </local:DayTemplateSelector.WeekendDayTemplate>
        
        <local:DayTemplateSelector.TodayTemplate>
            <DataTemplate>
                <!-- Today template -->
            </DataTemplate>
        </local:DayTemplateSelector.TodayTemplate>
    </local:DayTemplateSelector>
</ContentPage.Resources>

<calendar:ProCalendarView 
    DayTemplateSelector="{StaticResource DaySelector}"/>
```

---

## ⚡ Performance Tips

### 1. Use Simple Templates

```xml
<!-- ✅ Good - Simple and fast -->
<DataTemplate>
    <Label Text="{Binding Date, StringFormat='{0:dd}'}"/>
</DataTemplate>

<!-- ❌ Avoid - Complex nested layouts -->
<DataTemplate>
    <StackLayout>
        <Grid>
            <Frame>
                <StackLayout>
                    <!-- Too many nested layouts -->
                </StackLayout>
            </Frame>
        </Grid>
    </StackLayout>
</DataTemplate>
```

### 2. Lazy Load Events

```csharp
// Load events only for visible month
calendar.MonthChangedCommand = new Command<MonthChangedEventArgs>(
    async args => await LoadEventsForMonthAsync(args.NewDate));
```

### 3. Limit Events Per Day

```xml
<calendar:ProCalendarView 
    MaxEventsPerDay="3"
    ShowEvents="True"/>
```

---

## 🐛 Troubleshooting

### Calendar Not Rendering

1. Check namespace is correct
2. Ensure HeightRequest is set
3. Verify ControlTemplate has PART_DaysGrid

### Binding Not Working

1. Check BindingContext is set
2. Verify property names match
3. Use Mode=TwoWay for editable properties

### Templates Not Applying

1. Ensure DataTemplate is in Resources
2. Check x:Key matches StaticResource
3. Verify BindingContext is correct type

---

## 📚 Complete Example

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:calendar="clr-namespace:ProCalendar.Maui.Controls;assembly=ProCalendar.Maui"
             xmlns:vm="clr-namespace:MyApp.ViewModels"
             x:Class="MyApp.Views.CalendarPage">
    
    <ContentPage.BindingContext>
        <vm:CalendarViewModel />
    </ContentPage.BindingContext>
    
    <ContentPage.Resources>
        <ResourceDictionary>
            <!-- Custom Day Template -->
            <DataTemplate x:Key="CustomDayTemplate">
                <Border BackgroundColor="{Binding IsSelected, Converter={StaticResource BoolToColorConverter}}"
                        Padding="8">
                    <StackLayout>
                        <Label Text="{Binding Date, StringFormat='{0:dd}'}"
                               HorizontalOptions="Center"/>
                        <BoxView IsVisible="{Binding EventCount, Converter={StaticResource EventCountToVisibilityConverter}}"
                                 Color="Red"
                                 HeightRequest="4"
                                 WidthRequest="4"/>
                    </StackLayout>
                </Border>
            </DataTemplate>
        </ResourceDictionary>
    </ContentPage.Resources>
    
    <Grid RowDefinitions="*,Auto">
        <!-- Calendar -->
        <calendar:ProCalendarView 
            Grid.Row="0"
            DisplayDate="{Binding CurrentDate, Mode=TwoWay}"
            SelectedDate="{Binding SelectedDate, Mode=TwoWay}"
            EventsSource="{Binding Events}"
            DayTemplate="{StaticResource CustomDayTemplate}"
            DateSelectedCommand="{Binding DateSelectedCommand}"
            MonthChangedCommand="{Binding MonthChangedCommand}"
            TodayColor="Purple"
            SelectionColor="LightPurple"
            IsSwipeEnabled="True"
            ShowWeekNumbers="True"/>
        
        <!-- Actions -->
        <HorizontalStackLayout Grid.Row="1" Padding="10" Spacing="10">
            <Button Text="Load Events" Command="{Binding LoadEventsCommand}"/>
            <Button Text="Add Event" Command="{Binding AddEventCommand}"/>
            <Button Text="Clear" Command="{Binding ClearSelectionCommand}"/>
        </HorizontalStackLayout>
    </Grid>
    
</ContentPage>
```

---

## 🎯 Best Practices

1. **Always set HeightRequest** for proper rendering
2. **Use TwoWay binding** for SelectedDate, SelectedDates, SelectedRange
3. **Implement Commands** instead of event handlers for MVVM
4. **Keep templates simple** for better performance
5. **Lazy load events** per month for scalability
6. **Use Styles** for consistent appearance
7. **Test on multiple platforms** (Android, iOS, Windows, Mac)

---

**Happy Coding with ProCalendar.Maui!** 🚀
