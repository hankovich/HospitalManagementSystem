namespace Hms.UI.Infrastructure.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    public class ArrayToObjectMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var type = values.First().GetType();

            dynamic list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

            foreach (object value in values)
            {
                list.Add((dynamic)value);
            }

            return list;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
