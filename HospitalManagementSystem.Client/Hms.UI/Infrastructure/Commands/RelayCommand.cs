namespace Hms.UI.Infrastructure.Commands
{
    using System;
    using System.Windows.Input;

    public class RelayCommand<T> : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<T> _execute;

        public RelayCommand(Predicate<object> canExecute, Action<T> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public RelayCommand(Action<T> execute)
        {
            this._canExecute = null;
            this._execute = execute;
        }

        public RelayCommand(Action execute)
        {
            this._canExecute = null;
            this._execute = _ => execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (parameter is T)
            {
                this._execute((T)parameter);
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public RelayCommand(Action<object> execute)
        {
            this._canExecute = null;
            this._execute = execute;
        }

        public RelayCommand(Action execute)
        {
            this._canExecute = null;
            this._execute = _ => execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }
    }
}
