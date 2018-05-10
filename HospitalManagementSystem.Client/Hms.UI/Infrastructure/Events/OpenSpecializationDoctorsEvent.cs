namespace Hms.UI.Infrastructure.Events
{
    using Prism.Events;

    public class OpenSpecializationDoctorsEvent : PubSubEvent<OpenSpecializationDoctorsArgs>
    {
    }

    public class OpenSpecializationDoctorsArgs
    {
        public int SpecializationId { get; set; }

        public int PolyclinicId { get; set; }
    }
}