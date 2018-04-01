namespace Hms.UI
{
    using System.Windows;

    using Hms.Resolver;
    using Hms.UI.Infrastructure.Controls.Editors;
    using Hms.UI.Infrastructure.Providers;

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

            Kernel.Load(new NinjectModule[] { new CommonModule(), new ServiceModule() });

            Kernel.Bind<IDialogCoordinator>().ToConstant(DialogCoordinator.Instance);
            Kernel.Bind<ISuggestionProvider>().To<GeoSuggestionProvider>();
            Current.MainWindow = Kernel.Get<MainWindow>();
            Current.MainWindow.Show();
        }
    }
}