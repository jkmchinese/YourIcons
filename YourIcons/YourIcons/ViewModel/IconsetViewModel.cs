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
using ModernUI.Windows.Navigation;
using YourIcons.Model;
using YourIcons.View;

namespace YourIcons.ViewModel
{
    /// <summary>
    /// 图标集视图ViewModel
    /// </summary>
    public class IconsetViewModel : ViewModelBase
    {
        #region Fields

        private readonly IList<Iconset> m_iconsetList;
        private Iconset m_selectedIconset;

        #endregion

        #region Constructors

        public IconsetViewModel()
        {
            m_iconsetList = DataRetrieved.Instance.Iconsets;
            ViewIconsCmd = new RelayCommand(ViewIconsCmdExcute);
        }

        #endregion

        #region Properties

        public Iconset SelectedIconset
        {
            get
            {
                return m_selectedIconset;
            }
            set
            {
                if (m_selectedIconset != value)
                {
                    m_selectedIconset = value;
                    OnPropertyChanged(() => SelectedIconset);
                    HandleSelectedItemChanged();
                }
            }
        }

        public IEnumerable<Iconset> IconsetList { get { return m_iconsetList; } }

        public ICommand ViewIconsCmd { get; set; }

        #endregion

        #region Events

        #endregion

        #region Private Methods

        private void ViewIconsCmdExcute(object obj)
        {
            var modernWin = (Application.Current.MainWindow as ModernWindow);
            if (modernWin != null)
                modernWin.ContentSource = new Uri("/YourIcons;component/View/NewIconsView.xaml", UriKind.RelativeOrAbsolute);
        }

        private void HandleSelectedItemChanged()
        {
            ViewModelRetrived.Instance.NewIconsViewModelInstance.SetIconSet(SelectedIconset);
        }

        #endregion

        #region Protect Methods

        #endregion

        #region Public Methods

        #endregion

    }
}
