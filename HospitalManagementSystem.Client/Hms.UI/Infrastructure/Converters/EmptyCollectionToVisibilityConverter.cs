namespace Hms.UI.Infrastructure.Converters
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class EmptyCollectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility emptyCollectionResult = GetEmptyCollectionResult(parameter);
            Visibility notEmptyCollectionResult = (Visibility)(Visibility.Collapsed - emptyCollectionResult);

            var enumerable = value as IEnumerable;

            if (enumerable == null)
            {
                return emptyCollectionResult;
            }

            if (!enumerable.GetEnumerator().MoveNext())
            {
                return emptyCollectionResult;
            }

            return notEmptyCollectionResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static Visibility GetEmptyCollectionResult(object parameter)
        {
            if (parameter == null)
            {
                return Visibility.Collapsed;
            }

            try
            {
                return (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString(), true);
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }
    }
}
