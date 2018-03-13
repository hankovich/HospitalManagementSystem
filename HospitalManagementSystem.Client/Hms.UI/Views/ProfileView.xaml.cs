namespace Hms.UI.Views
{
    using System.Windows.Controls;

    using Hms.UI.ViewModels;

    using Ninject;

    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            this.InitializeComponent();
            this.DataContext = App.Kernel.Get<ProfileViewModel>();
        }
    }
}
