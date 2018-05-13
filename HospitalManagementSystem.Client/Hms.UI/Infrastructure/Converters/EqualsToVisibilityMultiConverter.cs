namespace Hms.UI.Infrastructure.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class EqualsToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object first = values[0];
            object second = values[1];

            if (first.Equals(second))
            {
                if (parameter?.ToString() == nameof(Equals))
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }

            if ((int)first < 0)
            {
                if (parameter?.ToString() == nameof(DependencyProperty.UnsetValue))
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }

            if (parameter == null)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
