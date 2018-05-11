namespace Hms.UI.Infrastructure.Events
{
    using Prism.Events;

    public class OpenDoctorTimetableEvent : PubSubEvent<OpenDoctorTimetableEventArgs>
    {
    }

    public class OpenDoctorTimetableEventArgs
    {
        public object ParentViewModel { get; set; }

        public int DoctorId { get; set; }
    }
}
