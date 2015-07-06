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
    /// <summary>
    /// 图标视图ViewModel
    /// </summary>
    public class NewIconsViewModel : ViewModelBase
    {
        #region Fields

        private IEnumerable<IconBase> m_iconList;
        private Icon m_selectedIcon;
        private ExportIconWindow m_exportWindow;
        private Model.Iconset m_iconset;

        #endregion

        #region Constructors

        public NewIconsViewModel()
        {
            CopyPathCmd = new RelayCommand(CopyPathCmdExcute);
            CopyPathDataCmd = new RelayCommand(CopyPathDataCmdExcute);
            ExportCmd = new RelayCommand(ExportCmdExcute);
            FavouriteCmd = new RelayCommand(FavouriteCmdExcute);
        }
        #endregion

        #region Properties

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

        public Iconset Iconset
        {
            get { return m_iconset; }
            set
            {
                if (m_iconset != value)
                {
                    m_iconset = value;
                    OnPropertyChanged(() => this.Iconset);
                    HandleIconset();
                }
            }
        }

        private void HandleIconset()
        {
            if (m_iconset != null)
            {
                IconList = m_iconset.Icons;
            }
        }

        public IEnumerable<IconBase> IconList
        {
            get
            {
                return m_iconList;
            }
            private set
            {
                m_iconList = value;
                OnPropertyChanged(() => this.IconList);
            }
        }

        public ICommand CopyPathCmd { get; set; }
        public ICommand CopyPathDataCmd { get; set; }
        public ICommand ExportCmd { get; set; }
        public ICommand FavouriteCmd { get; set; }

        #endregion

        #region Events

        #endregion

        #region Private Methods

        private void CopyPathCmdExcute(object obj)
        {
            IconHelper.CopyIconPath(m_selectedIcon);
        }

        private void CopyPathDataCmdExcute(object obj)
        {
            IconHelper.CopyIconPathData(m_selectedIcon);
        }

        private void ExportCmdExcute(object obj)
        {
            m_exportWindow = new ExportIconWindow();
            m_exportWindow.DataContext = new ExportViewModel(m_exportWindow, m_selectedIcon);
            m_exportWindow.Show();
        }

        private void FavouriteCmdExcute(object obj)
        {
            DataRetrieved.Instance.FavoriteIcon(m_selectedIcon);
        }

        #endregion

        #region Protect Methods

        internal void SetIconSet(Iconset iconset)
        {
            Iconset = iconset;
        }

        #endregion

        #region Public Methods

        #endregion



    }
}
