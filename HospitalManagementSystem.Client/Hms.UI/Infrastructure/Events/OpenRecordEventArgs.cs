namespace Hms.UI.Infrastructure.Events
{
    public class OpenRecordEventArgs
    {
        public int RecordId { get; set; }

        public object ParentViewModel { get; set; }
    }
}