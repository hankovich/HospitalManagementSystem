namespace Hms.UI.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.UI.Annotations;
    using Hms.UI.Infrastructure.Commands;

    public class MedicalCardViewModel : INotifyPropertyChanged
    {
        public MedicalCardViewModel(IMedicalCardService service)
        {
            this.MedicalCardService = service;
            this.LoadedCommand = AsyncCommand.Create(async () => this.MedicalCard = await this.MedicalCardService.GetMedicalCardAsync(0));
        }

        public IMedicalCardService MedicalCardService { get; set; }

        public MedicalCard MedicalCard
        {
            get
            {
                return this.medicalCard;
            }

            set
            {
                if (medicalCard != value)
                {
                    this.medicalCard = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private MedicalCard medicalCard;

        public IAsyncCommand LoadedCommand { get; set; }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
