namespace Hms.UI.Infrastructure.Commands
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<CancellationToken, Task<TResult>> command;

        private readonly CancelAsyncCommand cancelCommand;

        private NotifyTaskCompletion<TResult> execution;

        private bool isRunning;

        public AsyncCommand(Func<CancellationToken, Task<TResult>> command)
        {
            this.command = command;
            this.cancelCommand = new CancelAsyncCommand();
        }

        public override bool CanExecute(object parameter)
        {
            return this.Execution == null || this.Execution.IsCompleted;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            this.cancelCommand.NotifyCommandStarting();
            this.IsRunning = true;

            try
            {
                this.Execution = new NotifyTaskCompletion<TResult>(this.command(this.cancelCommand.Token));
                this.RaiseCanExecuteChanged();

                if (this.Execution?.TaskCompletion != null)
                {
                    await this.Execution.TaskCompletion;
                }
            }
            finally
            {
                this.cancelCommand.NotifyCommandFinished();
                this.IsRunning = false;
                this.RaiseCanExecuteChanged();
            }
        }

        public override bool IsRunning
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

        public ICommand CancelCommand => this.cancelCommand;

        public NotifyTaskCompletion<TResult> Execution
        {
            get
            {
                return this.execution;
            }

            private set
            {
                this.execution = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource cts = new CancellationTokenSource();

            private bool commandExecuting;

            public CancellationToken Token => this.cts.Token;

            public void NotifyCommandStarting()
            {
                this.commandExecuting = true;
                if (!this.cts.IsCancellationRequested)
                {
                    return;
                }

                this.cts = new CancellationTokenSource();
                this.RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                this.commandExecuting = false;
                this.RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter)
            {
                return this.commandExecuting && !this.cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                this.cts.Cancel();
                this.RaiseCanExecuteChanged();
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

            private void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public class AsyncCommand<TInput, TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<TInput, CancellationToken, Task<TResult>> command;

        private readonly CancelAsyncCommand cancelCommand;

        private NotifyTaskCompletion<TResult> execution;

        private bool isRunning;

        public AsyncCommand(Func<TInput, Task<TResult>> command)
        {
            this.command = (input, token) => command(input);
            this.cancelCommand = new CancelAsyncCommand();
        }

        public AsyncCommand(Func<TInput, CancellationToken, Task<TResult>> command)
        {
            this.command = command;
            this.cancelCommand = new CancelAsyncCommand();
        }

        public override bool CanExecute(object parameter)
        {
            return this.Execution == null || this.Execution.IsCompleted;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            this.cancelCommand.NotifyCommandStarting();
            this.IsRunning = true;

            try
            {
                this.Execution =
                    new NotifyTaskCompletion<TResult>(this.command((TInput)parameter, this.cancelCommand.Token));
                this.RaiseCanExecuteChanged();

                if (this.Execution?.TaskCompletion != null)
                {
                    await this.Execution.TaskCompletion;
                }
            }
            finally
            {
                this.cancelCommand.NotifyCommandFinished();
                this.IsRunning = false;
                this.RaiseCanExecuteChanged();
            }
        }

        public override bool IsRunning
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

        public ICommand CancelCommand => this.cancelCommand;

        public NotifyTaskCompletion<TResult> Execution
        {
            get
            {
                return this.execution;
            }

            private set
            {
                this.execution = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource cts = new CancellationTokenSource();

            private bool commandExecuting;

            public CancellationToken Token => this.cts.Token;

            public void NotifyCommandStarting()
            {
                this.commandExecuting = true;
                if (!this.cts.IsCancellationRequested)
                {
                    return;
                }

                this.cts = new CancellationTokenSource();
                this.RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                this.commandExecuting = false;
                this.RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter)
            {
                return this.commandExecuting && !this.cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                this.cts.Cancel();
                this.RaiseCanExecuteChanged();
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

            private void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command)
        {
            return new AsyncCommand<object>(
                async _ =>
                {
                    await command();
                    return null;
                });
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(_ => command());
        }

        public static AsyncCommand<object> Create(Func<CancellationToken, Task> command)
        {
            return new AsyncCommand<object>(
                async token =>
                {
                    await command(token);
                    return null;
                });
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(command);
        }

        public static AsyncCommand<TInput, object> Create<TInput>(Func<TInput, Task> command)
        {
            return new AsyncCommand<TInput, object>(
                async _ =>
                {
                    await command(_);
                    return null;
                });
        }

        public static AsyncCommand<TInput, TResult> Create<TInput, TResult>(Func<TInput, Task<TResult>> command)
        {
            return new AsyncCommand<TInput, TResult>(command);
        }

        public static AsyncCommand<TInput, object> Create<TInput>(Func<TInput, CancellationToken, Task> command)
        {
            return new AsyncCommand<TInput, object>(
                async (input, token) =>
                {
                    await command(input, token);
                    return null;
                });
        }

        public static AsyncCommand<TInput, TResult> Create<TInput, TResult>(Func<TInput, CancellationToken, Task<TResult>> command)
        {
            return new AsyncCommand<TInput, TResult>(command);
        }
    }
}