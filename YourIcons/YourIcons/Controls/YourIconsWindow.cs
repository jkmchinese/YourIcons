using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ModernUI.Presentation;
using ModernUI.Windows.Controls;

namespace YourIcons.Controls
{
    /// <summary>
    /// Main Window
    /// </summary>
    public class YourIconsWindow : ModernWindow
    {
        public static readonly DependencyProperty ToolBarContentProperty =
            DependencyProperty.Register("ToolBarContent", typeof(ContentControl), typeof(YourIconsWindow), new PropertyMetadata(default(ContentControl)));

        public ContentControl ToolBarContent
        {
            get { return (ContentControl)GetValue(ToolBarContentProperty); }
            set { SetValue(ToolBarContentProperty, value); }
        }

        public YourIconsWindow()
        {
            this.DefaultStyleKey = typeof(YourIconsWindow);
        }
    }
}
