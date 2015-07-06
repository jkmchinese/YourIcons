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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModernUI.Windows.Controls;
using YourIcons.Converters;
using YourIcons.ViewModel;

namespace YourIcons.View
{
    /// <summary>
    /// Interaction logic for NavView.xaml
    /// </summary>
    public partial class NavView : UserControl
    {
        public NavView()
        {
            InitializeComponent();
            this.DataContext = ViewModelRetrived.Instance.NavViewModelInstance;
            //var mainWin = Application.Current.MainWindow as ModernWindow;
            //var enableBinding = new Binding("ContentSource");
            //enableBinding.Source = mainWin;
            //enableBinding.Converter = new NavSearchIsEnableConverter();
            //this.IconsViewStackPanel.SetBinding(UIElement.IsEnabledProperty, enableBinding);
        }
    }
}
