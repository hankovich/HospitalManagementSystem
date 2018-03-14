namespace Hms.UI
{
    using Hms.UI.ViewModels;

    using MahApps.Metro.Controls;

    using Ninject;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = App.Kernel.Get<NavigationViewModel>();
        }
    }
}
