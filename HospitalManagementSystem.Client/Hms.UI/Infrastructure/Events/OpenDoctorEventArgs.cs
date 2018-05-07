namespace Hms.UI.Infrastructure.Events
{
    using Hms.Common.Interface.Domain;

    public class OpenDoctorEventArgs
    {
        public int DoctorId { get; set; }

        public object ParentViewModel { get; set; }
    }
}