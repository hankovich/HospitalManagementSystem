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

            var chain = new RelayCommand(
                o => commands.All(command => command.CanExecute(o)),
                o =>
                {
                    foreach (var command in commands)
                    {
                        try
                        {
                            command.Execute(o);
                        }
                        catch
                        {
                            break;
                        }
                    }
                });

            return chain;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
