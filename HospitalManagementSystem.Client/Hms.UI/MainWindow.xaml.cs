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

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < 100; i++)
            {
                await this.Service.Do();
            }
        }
    }
}
