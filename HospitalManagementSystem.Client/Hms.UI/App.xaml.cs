namespace Hms.UI
{
    using System.Windows;

    using Hms.Resolver;

    using Ninject;
    using Ninject.Modules;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel Kernel { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Kernel = new StandardKernel();

            this.Kernel.Load(new NinjectModule[] { new MainModule(), new ServiceModule() });

            Current.MainWindow = this.Kernel.Get<MainWindow>();
            Current.MainWindow.Show();
        }
    }
}