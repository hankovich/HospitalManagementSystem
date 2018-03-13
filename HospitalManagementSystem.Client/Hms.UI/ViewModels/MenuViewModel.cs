namespace Hms.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Hms.UI.Annotations;

    public class MenuViewModel : INotifyPropertyChanged
    {
        public MenuViewModel()
        {
            this.MenuItems = new ObservableCollection<string> { "one", "two", "three", "four", "five", "six", "one", "two", "three", "four", "five", "six", "one", "two", "three", "four", "five", "six", "one", "two", "three", "four", "five", "six" };
        }

        private ObservableCollection<string> menuItems;

        public ObservableCollection<string> MenuItems
        {
            get
            {
                return this.menuItems;
            }

            set
            {
                if (this.menuItems != value)
                {
                    this.menuItems = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
