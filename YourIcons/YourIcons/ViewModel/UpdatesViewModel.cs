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
    public class UpdatesViewModel : ViewModelBase
    {
        private ListCollectionView m_collectionView;
        private IList<Icon> m_updateIconsList;
        private Icon m_selectedIcon;
        private Icon m_firstIcon;
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

        public IList<Icon> UpdateIconsList { get { return m_updateIconsList; } }

        public UpdatesViewModel()
        {
            //m_firstIcon = DataRetrieved.Instance.IconList.FirstOrDefault();
            var baseDateTime = DateTime.Parse("2014/11/9");
            var list = DataRetrieved.Instance.IconList.Where(o => o.CreatedTime > baseDateTime);

            m_updateIconsList = new ObservableCollection<Icon>(list);
            m_collectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(UpdateIconsList);
            m_collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            DataRetrieved.Instance.IconAdded += Instance_IconAdded;
            DataRetrieved.Instance.IconDeleted += Instance_IconDeleted;
            CopyPathCmd = new RelayCommand(CopyPathCmdExcute);
            CopyPathDataCmd = new RelayCommand(CopyPathDataCmdExcute);
            EditCmd = new RelayCommand(EditCmdExcute);
            ExportCmd = new RelayCommand(ExportCmdExcute);
            DeleteCmd = new RelayCommand(DeleteCmdExcute);
            FavouriteCmd = new RelayCommand(FavouriteCmdExcute);
        }

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
            throw new NotImplementedException();
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

        void Instance_IconDeleted(object sender, IconEventArgs e)
        {
            if (e.Icon.CreatedTime > m_firstIcon.CreatedTime)
            {
                m_updateIconsList.Remove(e.Icon);
            }
        }

        void Instance_IconAdded(object sender, IconEventArgs e)
        {
            //if (m_firstIcon == null)
            //    m_firstIcon = e.Icon;
            m_updateIconsList.Add(e.Icon);
        }
    }
}
