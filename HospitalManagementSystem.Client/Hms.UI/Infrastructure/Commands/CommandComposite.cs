namespace Hms.UI.Infrastructure.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    public sealed class CommandComposite : ICommand
    {
        private readonly List<ICommand> commands; 

        public CommandComposite(IEnumerable<ICommand> commands)
        {
            this.commands = new List<ICommand>();

            foreach (var command in commands)
            {
                command.CanExecuteChanged += this.CommandCanExecuteChanged;
                this.commands.Add(command);
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.commands.All(command => command.CanExecute(parameter));
        }

        public async void Execute(object parameter)
        {
            foreach (var command in this.commands)
            {
                try
                {
                    if (command is IAsyncCommand)
                    {
                        await ((IAsyncCommand)command).ExecuteAsync(parameter);
                    }
                    else
                    {
                        command.Execute(parameter);
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        private void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
