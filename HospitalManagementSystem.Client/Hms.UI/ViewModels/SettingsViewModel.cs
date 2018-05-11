namespace Hms.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Hms.UI.Infrastructure.Commands;

    using MahApps.Metro;

    public class SettingsViewModel : ViewModelBase
    {
        private bool isDarkThemeEnabled;

        private Accent baseColor;

        public SettingsViewModel()
        {
            this.IsDarkThemeEnabled = ThemeManager.DetectAppStyle().Item1.Name.EndsWith("Dark");

            this.BaseColors = new ObservableCollection<Accent>(ThemeManager.Accents);
            this.BaseColor = ThemeManager.DetectAppStyle().Item2;

            this.ThemeChangedCommand = new RelayCommand<bool>(
                isChecked =>
                {
                    var appTheme = ThemeManager.GetInverseAppTheme(ThemeManager.DetectAppStyle().Item1);
                    this.isDarkThemeEnabled = !this.isDarkThemeEnabled;
                    ThemeManager.ChangeAppTheme(Application.Current, appTheme.Name);

                    Properties.Settings.Default["ThemeName"] = appTheme.Name;
                    Properties.Settings.Default["ThemeResourceAddress"] = appTheme.Resources.Source.OriginalString;
                    Properties.Settings.Default.Save();
                });

            this.BaseColorChanged = new RelayCommand<Accent>(
                newBaseColor =>
                {
                    this.BaseColor = newBaseColor;
                    var appTheme = ThemeManager.DetectAppStyle().Item1;

                    ThemeManager.ChangeAppStyle(Application.Current, newBaseColor, appTheme);
                    Properties.Settings.Default["BaseColorName"] = newBaseColor.Name;
                    Properties.Settings.Default["BaseColorResourceAddress"] = newBaseColor.Resources.Source.OriginalString;
                    Properties.Settings.Default.Save();
                });
        }

        public Accent BaseColor
        {
            get
            {
                return this.baseColor;
            }

            set
            {
                this.baseColor = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Accent> BaseColors { get; }

        public ICommand ThemeChangedCommand { get; }

        public ICommand BaseColorChanged { get; }

        public bool IsDarkThemeEnabled
        {
            get
            {
                return this.isDarkThemeEnabled;
            }

            set
            {
                this.isDarkThemeEnabled = value;
                this.OnPropertyChanged();
            }
        }
    }
}
