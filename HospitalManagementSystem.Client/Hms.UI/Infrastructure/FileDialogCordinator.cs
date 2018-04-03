namespace Hms.UI.Infrastructure
{
    using System;

    using Microsoft.Win32;

    public class FileDialogCordinator : IFileDialogCoordinator
    {
        public string OpenFile(string caption, string filter = "All files (*.*)|*.*")
        {
            OpenFileDialog diag = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Title = caption,
                Filter = filter,
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true
            };

            if (diag.ShowDialog() == true)
            {
                return diag.FileName;
            }

            return string.Empty;
        }
    }
}