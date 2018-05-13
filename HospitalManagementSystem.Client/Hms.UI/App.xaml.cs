namespace Hms.UI
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    using Hms.Resolver;
    using Hms.UI.Infrastructure;
    using Hms.UI.Infrastructure.Controls.Editors;
    using Hms.UI.Infrastructure.Providers;

    using MahApps.Metro;
    using MahApps.Metro.Controls.Dialogs;

    using Ninject;
    using Ninject.Modules;

    using Prism.Events;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IKernel Kernel { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => { };

            Dispatcher.CurrentDispatcher.UnhandledException += (sender, args) => { args.Handled = true; };

            Kernel = new StandardKernel();

            Kernel.Load(new NinjectModule[] { new CommonModule(), new ServiceModule() });

            Kernel.Bind<IDialogCoordinator>().ToConstant(DialogCoordinator.Instance);
            Kernel.Bind<IFileDialogCoordinator>().To<FileDialogCordinator>();

            Kernel.Bind<ISuggestionProvider>().To<GeoSuggestionProvider>();
            Kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            var themeName = (string)UI.Properties.Settings.Default["ThemeName"];
            var themeResourceAddress = (string)UI.Properties.Settings.Default["ThemeResourceAddress"];

            var baseColorName = (string)UI.Properties.Settings.Default["BaseColorName"];
            var baseColorResourceAddress = (string)UI.Properties.Settings.Default["BaseColorResourceAddress"];

            ThemeManager.ChangeAppStyle(
                App.Current,
                new Accent(baseColorName, new Uri(baseColorResourceAddress)),
                new AppTheme(themeName, new Uri(themeResourceAddress)));

            Current.MainWindow = Kernel.Get<MainWindow>();
            Current.MainWindow.Show();
        }
    }
}