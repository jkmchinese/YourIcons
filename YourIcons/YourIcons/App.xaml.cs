using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ModernUI.Presentation;

namespace YourIcons
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppearanceManager.Current.AccentColor = YourIcons.Properties.Settings.Default.AccentColor;
            AppearanceManager.Current.ThemeSource = YourIcons.Properties.Settings.Default.Theme;
            AppearanceManager.Current.FontSize = YourIcons.Properties.Settings.Default.FontSize == "large"
                ? FontSize.Large
                : FontSize.Small;
        }
    }
}
