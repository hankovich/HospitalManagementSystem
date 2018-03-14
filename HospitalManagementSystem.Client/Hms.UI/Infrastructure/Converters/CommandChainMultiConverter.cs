namespace Hms.UI.Infrastructure.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Input;

    using Hms.UI.Infrastructure.Commands;

    public class CommandChainMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));    
            }

            var commands = values.OfType<ICommand>();

            var chain = new CommandComposite(commands);

            return chain;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
