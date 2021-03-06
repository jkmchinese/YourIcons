﻿using System;
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
    /// Interaction logic for ExportIconWindow.xaml
    /// </summary>
    public partial class ExportIconWindow : ModernWindow
    {
        public ExportIconWindow()
        {
            InitializeComponent();
            Loaded += ExportIconWindow_Loaded;
        }

        void ExportIconWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var exportViewModel = this.DataContext as ExportViewModel;
            if (exportViewModel != null)
                exportViewModel.ExportCanvas = this.ExportCanvas;
        }
    }
}
