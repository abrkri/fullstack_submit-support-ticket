using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace MyApp.Admin.WinUI;

public sealed class ReportStateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) => value is true ? "Lezárva" : "Nyitott";
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException();
}

public sealed class OpenReportVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) => value is true ? Visibility.Collapsed : Visibility.Visible;
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException();
}

public sealed class ErrorBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) => new SolidColorBrush(value is true ? Colors.Firebrick : Colors.DarkSlateGray);
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException();
}