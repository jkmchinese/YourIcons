using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ModernUI.Presentation;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    public class NavViewModel : ViewModelBase
    {
        private string m_searchStr;
        private ICommand m_addIconCmd;
        private IconEntityWindow m_addIconWindow;

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

        public ICommand AddIconCmd
        {
            get
            {
                return m_addIconCmd;
            }
        }

        public NavViewModel()
        {
            m_addIconCmd = new RelayCommand(AddIconCmdExcute, CanAddIconCmdExcute);
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
