namespace Hms.UI.ViewModels
{
    using Prism.Events;

    public class SpecializationDoctorsViewModel : ViewModelBase
    {
        private int polyclinicId;

        private int specializationId;

        public SpecializationDoctorsViewModel(int polyclinicId, int specializationId, IEventAggregator eventAggregator)
        {
            this.PolyclinicId = polyclinicId;
            this.SpecializationId = specializationId;
            this.EventAggregator = eventAggregator;
        }

        public int PolyclinicId
        {
            get
            {
                return this.polyclinicId;
            }

            set
            {
                this.polyclinicId = value;
                this.OnPropertyChanged();
            }
        }

        public int SpecializationId
        {
            get
            {
                return this.specializationId;
            }

            set
            {
                this.specializationId = value;
                this.OnPropertyChanged();
            }
        }

        public IEventAggregator EventAggregator { get; }
    }
}
