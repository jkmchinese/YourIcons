using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
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

        private ObservableCollection<Icon> m_importIcons = new ObservableCollection<Icon>();
        private IList m_selectedItems;


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
                //RecursiveRead(doc);
                IEnumerable<XElement> pathList = doc.Descendants("Path");
                foreach (var xElement in pathList)
                {
                    var icon = DataRetrieved.Instance.GetIconFromElement(xElement);
                    if (icon != null)
                    {
                        m_importIcons.Add(icon);
                    }
                    else
                    {
                        Debug.WriteLine("Path Error:" + xElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadFile Error:" + ex);
            }
        }



        private void CancelCmdExcute(object obj)
        {
            m_window.Close();
        }

    }
}
