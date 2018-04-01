namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class NotifyDataErrorInfoBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public bool HasErrors => this.errors.Values.FirstOrDefault(l => l.Count > 0) != null;

        protected void AddError(string error = "Field must not be empty", [CallerMemberName]string propertyName = null)
        {
            if (!this.errors.ContainsKey(propertyName))
            {
                this.errors[propertyName] = new List<string>();
            }

            if (!this.errors[propertyName].Contains(error))
            {
                this.errors[propertyName].Add(error);
                this.OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors([CallerMemberName]string propertyName = null)
        {
            if (this.errors.ContainsKey(propertyName))
            {
                this.errors.Remove(propertyName);
                this.OnErrorsChanged(propertyName);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return this.errors.FirstOrDefault(error => error.Key == propertyName).Value;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnErrorsChanged([CallerMemberName]string propertyName = null)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}