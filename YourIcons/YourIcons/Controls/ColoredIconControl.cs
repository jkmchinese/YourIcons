using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using YourIcons.Model;

namespace YourIcons.Controls
{
    /// <summary>
    /// 图标控件
    /// </summary>
    [TemplatePart(Name = ELEMENT_CANVAS, Type = typeof(Canvas))]
    public class ColoredIconControl : Control
    {

        #region Fields

        /// <summary>
        /// The name for the Canvas part.
        /// </summary>
        private const string ELEMENT_CANVAS = "Canvas";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public static readonly DependencyProperty ColoredIconProperty =
            DependencyProperty.Register("ColoredIcon", typeof(ColoredIcon), typeof(ColoredIconControl), new PropertyMetadata(default(ColoredIcon), OnColoredIconChanged));

        public ColoredIcon ColoredIcon
        {
            get { return (ColoredIcon)GetValue(ColoredIconProperty); }
            set { SetValue(ColoredIconProperty, value); }
        }


        /// <summary>
        /// Canvas
        /// </summary>
        public Canvas Canvas { get; private set; }

        #endregion

        #region Events

        #endregion

        #region Private Methods

        private static void OnColoredIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var coloredIconControl = d as ColoredIconControl;
            var coloredIcon = e.NewValue as ColoredIcon;
            if (coloredIconControl == null || coloredIcon == null || coloredIconControl.Canvas == null)
            {
                return;
            }

            coloredIconControl.Canvas.Children.Clear();
            foreach (ColoredIconItem item in coloredIcon.Paths)
            {
                Path p = new Path()
                {
                    Data = Geometry.Parse(item.Data) as PathGeometry,
                    Width = item.Width,
                    Height = item.Height,
                    Fill = GetBrush(item.Fill),
                    Stroke = GetBrush(item.Stroke),
                    StrokeThickness = item.StrokeThickness,
                    Opacity = item.Opacity,
                };
                p.SetValue(Canvas.TopProperty, item.Top);
                p.SetValue(Canvas.LeftProperty, item.Left);
                coloredIconControl.Canvas.Children.Add(p);
            }
        }

        private static SolidColorBrush GetBrush(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
            {
                return new SolidColorBrush(Colors.Transparent);
            }
            var color = ColorConverter.ConvertFromString(hexStr);
            if (color != null)
                return new SolidColorBrush((Color)color);
            return new SolidColorBrush(Colors.Transparent);
        }

        #endregion

        #region Protect Methods

        #endregion

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Canvas = GetTemplateChild(ELEMENT_CANVAS) as Canvas;

        }

        #endregion
    }
}
