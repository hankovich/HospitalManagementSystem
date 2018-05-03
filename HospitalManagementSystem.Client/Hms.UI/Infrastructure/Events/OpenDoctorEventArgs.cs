namespace Hms.UI.Infrastructure.Events
{
    using Hms.Common.Interface.Domain;

    public class OpenDoctorEventArgs
    {
        public Doctor Doctor { get; set; }

        public object ParentViewModel { get; set; }
    }
}