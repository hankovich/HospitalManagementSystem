namespace Hms.UI.Wrappers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
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
            PropertyInfo info = typeof(T).GetProperty(propertyName);
            var currentValue = info.GetValue(this.Model);

            if (!currentValue?.Equals(value) ?? true)
            {
                info.SetValue(this.Model, value);
                this.OnPropertyChanged(propertyName);

                this.ValidatePropertyInternal(propertyName);
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }

        protected void RegisterCollection<TWrapper, TModel>(
            ObservableCollection<TWrapper> wrapperCollection,
            ICollection<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (sender, args) =>
            {
                modelCollection.Clear();

                foreach (var model in wrapperCollection.Select(w => w.Model))
                {
                    modelCollection.Add(model);
                }
            };
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

    }
}