namespace Hms.UI
{
    using System.Windows;

    using Hms.Resolver;

    using MahApps.Metro.Controls.Dialogs;

    using Ninject;
    using Ninject.Modules;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IKernel Kernel { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Kernel = new StandardKernel();

            Kernel.Load(new NinjectModule[] { new MainModule(), new ServiceModule() });

            Kernel.Bind<IDialogCoordinator>().ToConstant(DialogCoordinator.Instance);

            Current.MainWindow = Kernel.Get<MainWindow>();
            Current.MainWindow.Show();
        }
    }
}