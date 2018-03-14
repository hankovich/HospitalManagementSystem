namespace Hms.UI.Views
{
    using System.Windows.Controls;

    using Hms.UI.ViewModels;

    using Ninject;

    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            this.InitializeComponent();
            this.DataContext = App.Kernel.Get<LoginViewModel>();
        }
    }
}
