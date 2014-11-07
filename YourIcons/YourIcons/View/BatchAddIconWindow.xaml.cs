using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ModernUI.Windows.Controls;
using YourIcons.Model;
using YourIcons.ViewModel;

namespace YourIcons.View
{
    /// <summary>
    /// Interaction logic for BatchAddIconWindow.xaml
    /// </summary>
    public partial class BatchAddIconWindow : ModernWindow
    {
        private BatchAddIconViewModel m_viewModel;

        public BatchAddIconWindow()
        {
            InitializeComponent();
            Loaded += BatchAddIconWindow_Loaded;
        }

        void BatchAddIconWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_viewModel == null)
            {
                m_viewModel = (this.DataContext) as BatchAddIconViewModel;
            }
        }

        private void IconsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_viewModel.SelectedItems = this.IconsDataGrid.SelectedItems;
        }
    }
}
