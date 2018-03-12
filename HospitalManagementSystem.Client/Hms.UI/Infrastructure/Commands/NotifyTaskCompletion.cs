namespace Hms.UI.Infrastructure.Commands
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        public NotifyTaskCompletion(Task<TResult> task)
        {
            this.Task = task;
            if (!task.IsCompleted)
            {
                this.TaskCompletion = this.WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }

            var propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }

            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));

            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public Task<TResult> Task { get; }

        public Task TaskCompletion { get; }

        public TResult Result => (this.Task.Status == TaskStatus.RanToCompletion) ? this.Task.Result : default(TResult);

        public TaskStatus Status => this.Task.Status;

        public bool IsCompleted => this.Task.IsCompleted;

        public bool IsNotCompleted => !this.Task.IsCompleted;

        public bool IsSuccessfullyCompleted => this.Task.Status == TaskStatus.RanToCompletion;

        public bool IsCanceled => this.Task.IsCanceled;

        public bool IsFaulted => this.Task.IsFaulted;

        public AggregateException Exception => this.Task.Exception;

        public Exception InnerException => this.Exception?.InnerException;

        public string ErrorMessage => this.InnerException?.Message;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}