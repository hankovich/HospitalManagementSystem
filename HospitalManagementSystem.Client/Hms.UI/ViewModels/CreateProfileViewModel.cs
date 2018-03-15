namespace Hms.UI.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Hms.UI.Annotations;

    public class CreateProfileViewModel : INotifyPropertyChanged
    {
        private string name;

        private string selectedState;

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public string SelectedState
        {
            get
            {
                return this.selectedState;
            }

            set
            {
                this.selectedState = value;
                this.OnPropertyChanged();
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
