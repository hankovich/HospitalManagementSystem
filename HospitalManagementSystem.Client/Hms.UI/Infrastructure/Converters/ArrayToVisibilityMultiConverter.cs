namespace Hms.UI.Infrastructure.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    using Hms.UI.Infrastructure.Commands;

    public class ArrayToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = values.Select(this.ToBool).Aggregate(true, (current, previous) => current && previous);
            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool ToBool(object arg)
        {
            if (arg is bool)
            {
                return (bool)arg;
            }

            if (arg is int)
            {
                return (int)arg >= 0;
            }

            if (arg is Visibility)
            {
                return (Visibility)arg == Visibility.Visible;
            }

            NotifyTaskCompletion<object> completion = arg as NotifyTaskCompletion<object>;
            if (completion != null)
            {
                return completion.IsCompleted;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
