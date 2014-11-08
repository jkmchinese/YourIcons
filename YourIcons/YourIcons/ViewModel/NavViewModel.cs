using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ModernUI.Presentation;
using YourIcons.Model;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    public class NavViewModel : ViewModelBase
    {
        private string m_searchStr;
        private readonly ICommand m_addIconCmd;
        private readonly ICommand m_batchAddIconCmd;
        private readonly ICommand m_changeViewTypeCmd;
        private ViewType m_selectedViewType;
        private IconEntityWindow m_addIconWindow;
        private BatchAddIconWindow m_batchAddIconWindow;

        public string SearchStr
        {
            get
            {
                return m_searchStr;
            }
            set
            {
                if (m_searchStr != value)
                {
                    m_searchStr = value;
                    OnPropertyChanged(() => this.SearchStr);
                    OnSearchStrChanged();
                }
            }
        }

        public ViewType SelectedViewType
        {
            get
            {
                return m_selectedViewType;
            }
            set
            {
                if (m_selectedViewType != value)
                {
                    m_selectedViewType = value;
                    OnPropertyChanged(() => this.SelectedViewType);
                }
            }
        }

        public ICommand AddIconCmd
        {
            get
            {
                return m_addIconCmd;
            }
        }

        public ICommand BatchAddIconCmd
        {
            get
            {
                return m_batchAddIconCmd;
            }
        }

        public ICommand ChangeViewTypeCmd
        {
            get
            {
                return m_changeViewTypeCmd;
            }
        }


        public NavViewModel()
        {
            m_addIconCmd = new RelayCommand(AddIconCmdExcute, CanAddIconCmdExcute);
            m_batchAddIconCmd = new RelayCommand(BatchAddIconCmdExcute, CanBatchAddIconCmdExcute);
            m_changeViewTypeCmd = new RelayCommand(ChangeViewTypeCmdExcute, CanChangeViewTypeCmdExcute);
        }

        private bool CanChangeViewTypeCmdExcute(object arg)
        {
            return true;
        }

        private void ChangeViewTypeCmdExcute(object obj)
        {
            if (obj.ToString() == "Tile")
            {
                SelectedViewType = ViewType.Tile;
            }
            else
            {
                SelectedViewType = ViewType.List;
            }
        }

        private bool CanBatchAddIconCmdExcute(object arg)
        {
            return true;
        }

        private void BatchAddIconCmdExcute(object obj)
        {
            m_batchAddIconWindow = new BatchAddIconWindow();
            m_batchAddIconWindow.DataContext = new BatchAddIconViewModel(m_batchAddIconWindow);
            m_batchAddIconWindow.Show();
        }

        private bool CanAddIconCmdExcute(object arg)
        {
            return true;
        }

        private void AddIconCmdExcute(object obj)
        {
            //if (m_addIconWindow == null)
            //{
            m_addIconWindow = new IconEntityWindow();
            m_addIconWindow.DataContext = new IconEntityViewModel(m_addIconWindow);
            m_addIconWindow.Owner = Application.Current.MainWindow;
            //}
            m_addIconWindow.ShowDialog();
        }

        private void OnSearchStrChanged()
        {
            if (SearchStrChanged != null)
            {
                SearchStrChanged(this, new EventArgs());
            }
        }

        public event EventHandler<EventArgs> SearchStrChanged;
    }
}
