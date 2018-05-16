namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;

    using Prism.Events;

    public class MenuViewModel : ViewModelBase
    {
        private ObservableCollection<MenuItem> menuItems;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
            this.MenuItems = new ObservableCollection<MenuItem>();

            var rnd = new Random();

            this.MenuItems.Add(new MenuItem
            {
                Name = "Medical Card",
                ViewModelName = "MedicalCardViewModel",
                Badge = null
            });

            this.MenuItems.Add(new MenuItem
            {
                Name = "Doctor's appointment",
                ViewModelName = "AppointmentViewModel",
                Badge = null
            });

            this.MenuItems.Add(new MenuItem
            {
                Name = "My appointments",
                ViewModelName = "SettingsViewModel",
                Badge = 3
            });

            this.MenuItems.Add(new MenuItem
            {
                Name = "My tests",
                ViewModelName = "SettingsViewModel",
                Badge = 1
            });

            this.MenuItems.Add(new MenuItem
            {
                Name = "Seack leave info",
                ViewModelName = "SettingsViewModel",
                Badge = null
            });

            this.MenuItems.Add(new MenuItem
            {
                Name = "Settings",
                ViewModelName = "SettingsViewModel",
                Badge = null
            });

            this.MenuItems.Add(new MenuItem
            {
                Name = "Notifications",
                ViewModelName = "SettingsViewModel",
                Badge = 33
            });

            //for (int i = 0; i < 20; i++)
            //{
            //    this.MenuItems.Add(new MenuItem
            //    {
            //        Name = $"Koala_{rnd.Next() % 19 + 1}",
            //        Badge = rnd.NextDouble() > 0.5 ? (int?)null : rnd.Next() % 20
            //    });    
            //}

            this.OpenMenuItem =
                new RelayCommand<string>(vm => this.EventAggregator.GetEvent<OpenMenuItemEvent>().Publish(vm));
        }

        public IEventAggregator EventAggregator { get; }

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

        public ICommand OpenMenuItem { get; }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        
        public string ViewModelName { get; set; }

        public int? Badge { get; set; }
    }
}
