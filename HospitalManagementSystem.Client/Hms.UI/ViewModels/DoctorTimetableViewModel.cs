namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;
    using Hms.UI.Infrastructure;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;
    using Hms.UI.Wrappers;

    using Prism.Events;

    public class DoctorTimetableViewModel : ViewModelBase
    {
        private ProfileWrapper profile;

        private int userId;

        private DateTime selectedDate;

        public DoctorTimetableViewModel(int doctorId, object parentViewModel, IAppointmentDataService appointmentDataService, IProfileDataService profileDataService, INotificationService notificationService, IEventAggregator eventAggregator, IRequestCoordinator requestCoordinator)
        { 
            this.DoctorId = doctorId;
            this.ParentViewModel = parentViewModel;
            this.AppointmentDataService = appointmentDataService;
            this.ProfileDataService = profileDataService;
            this.NotificationService = notificationService;
            this.EventAggregator = eventAggregator;
            
            this.Appointments = new TrulyObservableCollection<CalendarItemWrapper>();

            this.UserId = requestCoordinator.UserId.Value;

            this.LoadedCommand = AsyncCommand.Create(this.OnLoadedAsync);
            this.SelectedDateChangedCommand = AsyncCommand.Create<DateTime>(this.OnSelectedDateChangedAsync);
            this.ScheduleAppointmentCommand = AsyncCommand.Create<DateTime>(this.OnScheduleAsync);
            this.CancelAppointmentCommand = AsyncCommand.Create<int>(this.OnCancelAsync);

            this.OpenDoctorCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<OpenDoctorEvent>().Publish(
                    new OpenDoctorEventArgs
                    {
                        DoctorId = this.DoctorId,
                        ParentViewModel = this
                    }));

            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        private async Task OnSelectedDateChangedAsync(DateTime oldDate)
        {
            await this.NotificationService.UnsubscribeAsync(this.DoctorId, oldDate);
            await this.NotificationService.SubscribeAsync(this.DoctorId, this.SelectedDate, async () => await this.UpdateAppointments());

            await this.UpdateAppointments();
        }

        private async Task UpdateAppointments()
        {
            this.Appointments.Clear();

            var appointments = await this.AppointmentDataService.GetAppointmentsAsync(this.DoctorId, this.SelectedDate);

            for (int i = 0; i < (18 - 8) * 4; i++)
            {
                var dateTime = this.SelectedDate.Date + TimeSpan.FromHours(8) + TimeSpan.FromMinutes(15 * i);

                var appointment = appointments?.FirstOrDefault(ap => ap.StartDate == dateTime);

                if (appointment != null)
                {
                    this.Appointments.Add(new CalendarItemWrapper(appointment));
                }
                else
                {
                    this.Appointments.Add(new CalendarItemWrapper(new CalendarItem { StartDate = dateTime }));
                }
            }
        }

        private async Task OnScheduleAsync(DateTime startDate)
        {
            int appointmentId = await this.AppointmentDataService.ScheduleAppointmentAsync(this.UserId, this.DoctorId, this.SelectedDate, startDate);

            Dictionary<int, CalendarItemWrapper> items = new Dictionary<int, CalendarItemWrapper>();

            foreach (CalendarItemWrapper item in this.Appointments.Where(ap => ap.StartDate == startDate))
            {
                var itemIndex = this.Appointments.IndexOf(item);
                item.Owner = new UserWrapper(new User
                {
                    Id = this.UserId
                });

                items[itemIndex] = item;
            }

            foreach (var item in items)
            {
                item.Value.Id = appointmentId;

                this.Appointments.RemoveAt(item.Key);
                this.Appointments.Insert(item.Key, item.Value);
            }
        }

        private async Task OnCancelAsync(int appointmentId)
        {
            await this.AppointmentDataService.CancelAppointmentAsync(appointmentId);

            Dictionary<int, CalendarItemWrapper> items = new Dictionary<int, CalendarItemWrapper>();

            foreach (CalendarItemWrapper item in this.Appointments.Where(ap => ap.Id == appointmentId))
            {
                var itemIndex = this.Appointments.IndexOf(item);
                item.Owner = new UserWrapper(null);

                items[itemIndex] = item;
            }

            foreach (var item in items)
            {
                this.Appointments.RemoveAt(item.Key);
                this.Appointments.Insert(item.Key, item.Value);
            }
        }

        private async Task OnLoadedAsync()
        {
            var profileModel = await this.ProfileDataService.GetProfileAsync(this.DoctorId);
            this.Profile = new ProfileWrapper(profileModel);

            this.SelectedDate = DateTime.Now;
        }

        public ProfileWrapper Profile
        {
            get
            {
                return this.profile;
            }

            set
            {
                this.profile = value;
                this.OnPropertyChanged();
            }
        }

        public int UserId
        {
            get
            {
                return this.userId;
            }

            set
            {
                this.userId = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return this.selectedDate;
            }

            set
            {
                this.selectedDate = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand LoadedCommand { get; }

        public ICommand SelectedDateChangedCommand { get; }

        public ICommand CancelAppointmentCommand { get; }

        public ICommand ScheduleAppointmentCommand { get; }

        public ICommand OpenDoctorCommand { get; }

        public ICommand BackCommand { get; }

        public int DoctorId { get; }

        public object ParentViewModel { get; }

        public IAppointmentDataService AppointmentDataService { get; set; }

        public IProfileDataService ProfileDataService { get; }

        public INotificationService NotificationService { get; }

        public IEventAggregator EventAggregator { get; }

        public ObservableCollection<CalendarItemWrapper> Appointments { get; } 
    }
}
