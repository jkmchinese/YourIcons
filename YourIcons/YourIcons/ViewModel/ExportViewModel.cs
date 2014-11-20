using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using ModernUI.Presentation;
using ModernUI.Windows.Controls;
using YourIcons.Model;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    public class ExportViewModel : ViewModelBase
    {
        private ExportIconWindow m_window;
        private string m_filePath;
        private Icon m_exportIcon;
        private IconSize[] m_iconSizes =
        {
            new IconSize(16,16),
            new IconSize(32,32),
            new IconSize(48,48),
            new IconSize(64,64),
            new IconSize(96,96),
        };
        private IconSize m_selectedIconSize;
        private Color m_foregroundColor;
        private Color m_backgroundColor;
        private int m_margin;

        public ExportViewModel(ExportIconWindow window, Icon exportIcon)
        {
            m_window = window;
            m_exportIcon = exportIcon;
            ExportPngCommand = new RelayCommand(ExportPngCmdExcute);
            ExportPathCommand = new RelayCommand(ExportPathCmdExcute);
            CancelCommand = new RelayCommand(CancelCmdExcute);
            m_selectedIconSize = m_iconSizes[2];
            m_foregroundColor = Colors.DimGray;
            m_backgroundColor = Colors.Transparent;
            m_margin = 2;
        }

        public ICommand ExportPngCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand ExportPathCommand { get; private set; }


        public IconSize[] IconSizes
        {
            get { return this.m_iconSizes; }
        }

        public IconSize SelectedIconSize
        {
            get { return this.m_selectedIconSize; }
            set
            {
                if (this.m_selectedIconSize != value)
                {
                    this.m_selectedIconSize = value;
                    OnPropertyChanged("SelectedIconSize");
                    OnPropertyChanged("ExportPath");
                    OnPropertyChanged("ExportPathWidth");
                    OnPropertyChanged("ExportPathHeight");
                }
            }
        }

        public string FilePath
        {
            get { return m_filePath; }
            set
            {
                m_filePath = value;
                RaisePropertyChanged("FilePath");
            }
        }

        public Icon ExportIcon
        {
            get { return m_exportIcon; }
            set
            {
                m_exportIcon = value;
                RaisePropertyChanged("ExportIcon");
            }
        }

        public Color ForegroundColor
        {
            get { return this.m_foregroundColor; }
            set
            {
                if (this.m_foregroundColor != value)
                {
                    this.m_foregroundColor = value;
                    OnPropertyChanged("ForegroundColor");
                    OnPropertyChanged("ExportPath");
                }
            }
        }

        public Color BackgroundColor
        {
            get { return this.m_backgroundColor; }
            set
            {
                if (this.m_backgroundColor != value)
                {
                    this.m_backgroundColor = value;
                    OnPropertyChanged("BackgroundColor");
                    OnPropertyChanged("ExportPath");
                }
            }
        }

        public int Margin
        {
            get { return this.m_margin; }
            set
            {
                if (this.m_margin != value)
                {
                    this.m_margin = value;
                    OnPropertyChanged("Margin");
                    OnPropertyChanged("ExportPath");
                    OnPropertyChanged("ExportPathWidth");
                    OnPropertyChanged("ExportPathHeight");
                }
            }
        }

        public int ExportPathWidth
        {
            get
            {
                return SelectedIconSize.Width - Margin;
            }
        }

        public int ExportPathHeight
        {
            get
            {
                return SelectedIconSize.Height - Margin;
            }
        }

        public Canvas ExportCanvas
        {
            get;
            set;
        }

        public String ExportPath
        {
            get
            {
                return GetExportPath().ToString();
            }
        }

        private XElement GetExportPath()
        {
            return new XElement("Canvas",
                new XAttribute("Width", SelectedIconSize.Width),
                new XAttribute("Height", SelectedIconSize.Height),
                new XAttribute("Background", BackgroundColor),
                new XElement("Path",
                     new XAttribute("Canvas.Top", Margin),
                    new XAttribute("Canvas.Left", Margin),
                    new XAttribute("Width", SelectedIconSize.Width - Margin),
                    new XAttribute("Height", SelectedIconSize.Height - Margin),
                    new XAttribute("Stretch", "Uniform"),
                    new XAttribute("Fill", ForegroundColor),
                    new XAttribute("Data", ExportIcon.Data))
                );
            //<Canvas Width="48"
            //        Height="48"
            //        Background="White">
            //    <Path Canvas.Top="{Binding Margin}"
            //          Canvas.Left="{Binding Margin}"
            //          Height="32"
            //          Width="32"
            //          Data="{Binding ExportIcon.Data}"
            //          Stretch="Uniform"
            //          Fill="Yellow"></Path>
            //</Canvas>
        }

        private void ExportPngCmdExcute(object obj)
        {
            var sfd = new Microsoft.Win32.SaveFileDialog();
            string exportDirectory = Path.Combine(Environment.CurrentDirectory, "Export");
            if (!Directory.Exists(exportDirectory))
            {
                Directory.CreateDirectory(exportDirectory);
            }
            sfd.InitialDirectory = exportDirectory;
            // Default file name
            sfd.FileName = m_exportIcon.Name;
            // Default file extension
            sfd.DefaultExt = ".png";
            // Filter files by extension
            sfd.Filter = "PNG Files |*.png";

            // Show save file dialog
            var result = sfd.ShowDialog();

            // Process save file dialog results
            if (result != true)
            {
                return;
            }
            // Save document
            string filename = sfd.FileName;
            bool exportResult = IconHelper.SavePng(ExportCanvas, ExportIcon, filename);
            if (exportResult)
            {
                ModernDialog.ShowMessage("Emport successfully.\r\nPath:" + filename,
                    "Operation Successful", MessageBoxButton.OK, m_window);

            }
            else
            {
                ModernDialog.ShowMessage("Emport failed.\r\n\r\nThe reason maybe you have not enought file write permition",
                   "Operation Failed", MessageBoxButton.OK, m_window);
            }
            //if (!DataRetrieved.Instance.BatchAddIcon(m_importIcons))
            //{
            //    ModernDialog.ShowMessage("BatchAddIcon occur some error,Some Icons has not import successfully.\r\nFor more information,please read the log.",
            //        "Warning", MessageBoxButton.OK);
            //}
            //m_window.Close();
        }

        private void ExportPathCmdExcute(object obj)
        {
            var sfd = new Microsoft.Win32.SaveFileDialog();
            string exportDirectory = Path.Combine(Environment.CurrentDirectory, "Export");
            if (!Directory.Exists(exportDirectory))
            {
                Directory.CreateDirectory(exportDirectory);
            }
            sfd.InitialDirectory = exportDirectory;
            // Default file name
            sfd.FileName = m_exportIcon.Name;
            // Default file extension
            sfd.DefaultExt = ".png";
            // Filter files by extension
            sfd.Filter = "Xaml Files |*.xaml";

            // Show save file dialog
            var result = sfd.ShowDialog();

            // Process save file dialog results
            if (result != true)
            {
                return;
            }
            // Save document
            string filename = sfd.FileName;
            bool exportResult = IconHelper.SavePathFile(GetExportPath(), ExportIcon, filename);
            if (exportResult)
            {
                ModernDialog.ShowMessage("Emport successfully.\r\nPath:" + filename,
                    "Operation Successful", MessageBoxButton.OK, m_window);

            }
            else
            {
                ModernDialog.ShowMessage("Emport failed.\r\n\r\nThe reason maybe you have not enought file write permition",
                   "Operation Failed", MessageBoxButton.OK, m_window);
            }
        }

        private void CancelCmdExcute(object obj)
        {
            m_window.Close();
        }
    }

    public class IconSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public IconSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return Width + "*" + Height;
        }
    }
}
