namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Hms.UI.Annotations;

    public class MenuViewModel : INotifyPropertyChanged
    {
        public MenuViewModel()
        {
            this.MenuItems = new ObservableCollection<MenuItem>();

            var rnd = new Random();

            for (int i = 0; i < 20; i++)
            {
                this.MenuItems.Add(new MenuItem
                {
                    Name = $"Koala_{rnd.Next() % 19 + 1}",
                    Badge = rnd.NextDouble() > 0.5 ? (int?)null : rnd.Next() % 20
                });    
            }
        }

        private ObservableCollection<MenuItem> menuItems;

        public ObservableCollection<MenuItem> MenuItems
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

    public class MenuItem
    {
        public string Name { get; set; }

        public int? Badge { get; set; }
    }
}
