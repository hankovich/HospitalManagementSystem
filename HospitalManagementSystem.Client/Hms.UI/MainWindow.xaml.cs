namespace Hms.UI
{
    using System.Windows;

    using Hms.Services.Interface;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IService Service { get; set; }

        public MainWindow(IService service)
        {
            this.Service = service;
            Visibility = Visibility.Hidden;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Service.Do();
        }
    }
}
