using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LWLCX.Framework.Common.Logger;
using ModernUI.Presentation;

namespace YourIcons
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private string m_fullVersion;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            m_fullVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            AppearanceManager.Current.AccentColor = YourIcons.Properties.Settings.Default.AccentColor;
            AppearanceManager.Current.ThemeSource = YourIcons.Properties.Settings.Default.Theme;
            AppearanceManager.Current.FontSize = YourIcons.Properties.Settings.Default.FontSize == "large"
                ? FontSize.Large
                : FontSize.Small;

            var ch = new CallHelper();
            LoggingService.Debug(GetEnvironmentInfo());
            this.DispatcherUnhandledException += Client_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LoggingService.Info("Starting youricons......" + m_fullVersion);
        }

        private string GetEnvironmentInfo()
        {
            var sb = new StringBuilder(1024);

            sb.Append("Current Environment:\n");
            sb.Append("OS Version:  "); sb.AppendLine(Environment.OSVersion.VersionString);
            sb.Append("Machine Name:    "); sb.AppendLine(Environment.MachineName);
            sb.Append("Processor Count: "); sb.AppendLine(Environment.ProcessorCount.ToString());
            sb.Append("Is 64bit Process:    "); sb.AppendLine(Environment.Is64BitProcess.ToString());
            sb.Append("Is 64bit OS: "); sb.AppendLine(Environment.Is64BitOperatingSystem.ToString());
            sb.Append("Page Size:   "); sb.AppendLine(Environment.SystemPageSize.ToString());
            sb.Append("UserName:    "); sb.AppendLine(Environment.UserName);
            sb.Append("WorkingSet:  "); sb.AppendLine(Environment.WorkingSet.ToString());
            sb.Append("CurrentDirecotry:    "); sb.AppendLine(Environment.CurrentDirectory);
            //sb.Append("StackTrace:  "); sb.AppendLine(Environment.StackTrace);

            sb.AppendLine("End Current Environment.");

            return sb.ToString();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            LoggingService.Fatal("Domain Unhandled Exception<" + m_fullVersion + ">:" + e.ExceptionObject);
            LoggingService.Fatal(GetEnvironmentInfo());
        }

        private void Client_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LoggingService.Fatal("Client Unhandled Exception<" + m_fullVersion + ">:" + e.Exception);
            LoggingService.Fatal(GetEnvironmentInfo());
            e.Handled = true;
        }

    }
}
