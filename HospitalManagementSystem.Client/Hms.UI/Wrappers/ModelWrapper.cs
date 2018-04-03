namespace Hms.UI.Wrappers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Hms.UI.ViewModels;

    public class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        public ModelWrapper(T model)
        {
            this.Model = model;
        } 

        public T Model { get; }

        public void Revalidate()
        {
            typeof(T).GetProperties().ToList().ForEach(pi => this.ValidatePropertyInternal(pi.Name));
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(this.Model);
        }

        protected virtual void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(this.Model, value);
            this.OnPropertyChanged(propertyName);

            this.ValidatePropertyInternal(propertyName);
        }

        private void ValidatePropertyInternal(string propertyName)
        {
            this.ClearErrors(propertyName);

            var errors = this.ValidateProperty(propertyName);

            if (errors != null)
            {
                foreach (var error in errors)
                {
                    this.AddError(error, propertyName);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}