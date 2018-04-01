namespace Hms.UI.ViewModels
{
    using System;
    using System.Windows.Threading;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += (sender, args) => this.OnPropertyChanged(nameof(Time));
            timer.Start();
        }

        public string Time => DateTime.Now.ToString("HH:mm:ss");
    }
}
