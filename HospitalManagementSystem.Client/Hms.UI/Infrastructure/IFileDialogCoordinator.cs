namespace Hms.UI.Infrastructure
{
    public interface IFileDialogCoordinator
    {
        string OpenFile(string caption, string filter = @"All files (*.*)|*.*");
    }
}
