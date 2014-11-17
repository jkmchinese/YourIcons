using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ModernUI.Presentation;
using ModernUI.Windows.Controls;
using YourIcons.Model;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    public class FavoriteViewModel : ViewModelBase
    {
        private ListCollectionView m_collectionView;
        private IList<Icon> m_iconLists;
        private Icon m_selectedIcon;
        private string m_searchStr;
        private IconEntityWindow m_eidtIconWindow;

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

        public IList<Icon> IconList { get { return m_iconLists; } }

        public FavoriteViewModel()
        {
            m_iconLists = new ObservableCollection<Icon>(DataRetrieved.Instance.IconList.Where(o => o.IsFavourite));
            DataRetrieved.Instance.IconModified += Instance_IconModified;
            CopyPathCmd = new RelayCommand(CopyPathCmdExcute);
            CopyPathDataCmd = new RelayCommand(CopyPathDataCmdExcute);
            EditCmd = new RelayCommand(EditCmdExcute);
            ExportCmd = new RelayCommand(ExportCmdExcute);
            DeleteCmd = new RelayCommand(DeleteCmdExcute);
            UnFavouriteCmd = new RelayCommand(UnFavouriteCmdExcute);
        }

        void Instance_IconModified(object sender, IconEventArgs e)
        {
            if (e.Icon.IsFavourite)
            {
                m_iconLists.Add(e.Icon);
            }
            else
            {
                m_iconLists.Remove(e.Icon);
            }
        }

        #region Command
        public ICommand CopyPathCmd { get; set; }
        public ICommand CopyPathDataCmd { get; set; }
        public ICommand EditCmd { get; set; }
        public ICommand ExportCmd { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand UnFavouriteCmd { get; set; }

        private void CopyPathCmdExcute(object obj)
        {
            IconHelper.CopyIconPath(m_selectedIcon);
        }

        private void CopyPathDataCmdExcute(object obj)
        {
            IconHelper.CopyIconPathData(m_selectedIcon);
        }

        private void EditCmdExcute(object obj)
        {
            m_eidtIconWindow = new IconEntityWindow();
            m_eidtIconWindow.DataContext = new IconEntityViewModel(m_selectedIcon, m_eidtIconWindow);
            m_eidtIconWindow.Owner = Application.Current.MainWindow;
            m_eidtIconWindow.ShowDialog();
        }

        private void ExportCmdExcute(object obj)
        {
            throw new NotImplementedException();
        }

        private void DeleteCmdExcute(object obj)
        {
            var result = ModernDialog.ShowMessage("Really to delete the Icon", "Operation Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                DataRetrieved.Instance.DeleteIcon(m_selectedIcon);
        }

        private void UnFavouriteCmdExcute(object obj)
        {
            DataRetrieved.Instance.UnFavoriteIcon(m_selectedIcon);
        }
        #endregion
    }
}
