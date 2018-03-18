namespace Hms.UI.Infrastructure.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using Hms.UI.Annotations;

    public sealed class CommandComposite : ICommand, INotifyPropertyChanged
    {
        private readonly List<ICommand> commands;

        private bool isRunning;

        public CommandComposite(IEnumerable<ICommand> commands)
        {
            this.commands = new List<ICommand>();

            foreach (var command in commands)
            {
                command.CanExecuteChanged += this.CommandCanExecuteChanged;
                this.commands.Add(command);
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
            set
            {
                this.isRunning = value;
                this.OnPropertyChanged();
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.commands.All(command => command.CanExecute(parameter));
        }

        public async void Execute(object parameter)
        {
            this.IsRunning = true;

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

            this.IsRunning = false;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
