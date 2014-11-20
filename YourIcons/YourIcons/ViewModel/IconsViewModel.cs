using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ModernUI.Presentation;
using ModernUI.Windows.Controls;
using YourIcons.Model;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    public class IconsViewModel : ViewModelBase
    {
        private ListCollectionView m_collectionView;
        private IList<Icon> m_iconLists;
        private Icon m_selectedIcon;
        private string m_searchStr;
        private IconEntityWindow m_eidtIconWindow;
        private ViewType m_currentViewType;
        private ExportIconWindow m_exportWindow;

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

        public ViewType CurrentViewType
        {
            get
            {
                return m_currentViewType;
            }
            set
            {
                if (m_currentViewType != value)
                {
                    m_currentViewType = value;
                    OnCurrentViewTypeChanged();
                    OnPropertyChanged(() => this.CurrentViewType);
                }
            }
        }

        private void OnCurrentViewTypeChanged()
        {

        }

        public IList<Icon> IconList { get { return m_iconLists; } }

        public IconsViewModel()
        {
            m_iconLists = DataRetrieved.Instance.IconList;
            m_collectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(IconList);
            m_collectionView.Filter = Filt;
            var navInstance = ViewModelRetrived.Instance.NavViewModelInstance;
            if (navInstance != null)
            {
                navInstance.PropertyChanged += NavViewModelInstance_PropertyChanged;
            }

            CopyPathCmd = new RelayCommand(CopyPathCmdExcute);
            CopyPathDataCmd = new RelayCommand(CopyPathDataCmdExcute);
            EditCmd = new RelayCommand(EditCmdExcute);
            ExportCmd = new RelayCommand(ExportCmdExcute);
            DeleteCmd = new RelayCommand(DeleteCmdExcute);
            FavouriteCmd = new RelayCommand(FavouriteCmdExcute);
        }

        #region Command
        public ICommand CopyPathCmd { get; set; }
        public ICommand CopyPathDataCmd { get; set; }
        public ICommand EditCmd { get; set; }
        public ICommand ExportCmd { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand FavouriteCmd { get; set; }

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
            //}
            m_eidtIconWindow.ShowDialog();
        }

        private void ExportCmdExcute(object obj)
        {
            m_exportWindow = new ExportIconWindow();
            m_exportWindow.DataContext = new ExportViewModel(m_exportWindow, m_selectedIcon);
            m_exportWindow.Show();
        }

        private void DeleteCmdExcute(object obj)
        {
            var result = ModernDialog.ShowMessage("Really to delete the Icon", "Operation Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                DataRetrieved.Instance.DeleteIcon(m_selectedIcon);
        }

        private void FavouriteCmdExcute(object obj)
        {
            DataRetrieved.Instance.FavoriteIcon(m_selectedIcon);
        }
        #endregion

        void NavViewModelInstance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchStr")
            {
                m_searchStr = (sender as NavViewModel).SearchStr;
                m_collectionView.Refresh();
            }
            else if (e.PropertyName == "SelectedViewType")
            {
                CurrentViewType = (sender as NavViewModel).SelectedViewType;
            }
        }

        private bool Filt(object item)
        {
            var k = item as Icon;
            if (!string.IsNullOrEmpty(m_searchStr) && k != null)
            {
                return Regex.IsMatch(k.Name, m_searchStr, RegexOptions.IgnoreCase) ||
                    (!string.IsNullOrEmpty(k.Keyword) && Regex.IsMatch(k.Keyword, m_searchStr, RegexOptions.IgnoreCase));
                //return k.Name.ToLower(CultureInfo.InvariantCulture).Contains(m_searchStr);
            }
            return true;
        }
    }
}
