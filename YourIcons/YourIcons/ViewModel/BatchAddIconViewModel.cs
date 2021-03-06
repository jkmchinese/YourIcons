﻿using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using LWLCX.Framework.Common.Logger;
using ModernUI.Presentation;
using ModernUI.Windows.Controls;
using MvvmValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourIcons.Model;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    public class BatchAddIconViewModel : ViewModelBase
    {
        private BatchAddIconWindow m_window;
        private string m_filePath;
        private ObservableCollection<Icon> m_importIcons = new ObservableCollection<Icon>();
        private IList m_selectedItems;
        private Icon m_selectedIcon;

        public BatchAddIconViewModel(BatchAddIconWindow window)
        {
            m_window = window;
            SaveCommand = new RelayCommand(SaveCmdExcute);
            CancelCommand = new RelayCommand(CancelCmdExcute);
            BrowseCmd = new RelayCommand(BroseCmdExcute);
            DeleteSelectedCommand = new RelayCommand(DeleteSelectedCmdExcute, CanDeleteSelectedCmdExcute);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand BrowseCmd { get; private set; }
        public ICommand DeleteSelectedCommand { get; private set; }

        public Icon SelectedIcon
        {
            get
            {
                return m_selectedIcon;
            }
            set
            {
                if (m_selectedIcon != value)
                {
                    m_selectedIcon = value;
                    OnPropertyChanged(() => this.SelectedIcon);
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

        private void SaveCmdExcute(object obj)
        {
            if (!DataRetrieved.Instance.BatchAddIcon(m_importIcons))
            {
                ModernDialog.ShowMessage("BatchAddIcon occur some error,Some Icons has not import successfully.\r\nFor more information,please read the log.",
                    "Warning", MessageBoxButton.OK);
            }
            m_window.Close();
        }

        private void DeleteSelectedCmdExcute(object obj)
        {
            var list = SelectedItems.Cast<Icon>().ToList();
            foreach (var selectedItem in list)
            {
                m_importIcons.Remove(selectedItem);
            }
        }

        private bool CanDeleteSelectedCmdExcute(object arg)
        {
            return m_selectedItems != null && m_selectedItems.Count > 0;
        }

        private void BroseCmdExcute(object obj)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.Filter = "xml file|*.xml";
            if (string.IsNullOrEmpty(m_filePath))
            {
                ofd.InitialDirectory = Environment.CurrentDirectory;
            }
            else
            {
                ofd.InitialDirectory = Path.GetDirectoryName(m_filePath);
            }
            if (ofd.ShowDialog() == true)
            {
                FilePath = ofd.FileName;
                ReadFile();
            }
        }

        public ObservableCollection<Icon> ImportIcons
        {
            get
            {
                return m_importIcons;
            }
        }

        public IList SelectedItems
        {
            get
            {
                return m_selectedItems;
            }
            set
            {
                if (!Equals(m_selectedItems, value))
                {
                    m_selectedItems = value;
                    RaisePropertyChanged("SelectedItems");
                }
            }
        }

        private void ReadFile()
        {
            m_importIcons.Clear();
            try
            {
                XElement doc = XElement.Load(m_filePath);
                IEnumerable<XElement> pathList = doc.Descendants("Path");
                foreach (var xElement in pathList)
                {
                    var icon = IconHelper.GetIconFromElement(xElement);
                    if (icon != null)
                    {
                        icon.CreatedTime = DateTime.Now;
                        m_importIcons.Add(icon);
                    }
                    else
                    {
                        LoggingService.Error("BatchAdd Path Error:" + xElement);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error("ReadFile Error:" + ex);
            }
        }

        private void CancelCmdExcute(object obj)
        {
            m_window.Close();
        }
    }
}
