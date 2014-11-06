using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using ModernUI.Presentation;
using Path = System.Windows.Shapes.Path;

namespace YourIcons.Model
{
    public static class IconHelper
    {
        public static bool CopyIconPath(Icon icon)
        {
            var xe = new XElement("Path",
                 new XAttribute("Name", icon.Name),
                 new XAttribute("Width", icon.Width),
                 new XAttribute("Height", icon.Height),
                 new XAttribute("Data", icon.Data)
                //new XAttribute("Keyword", icon.Keyword)
                 );
            try
            {
                Clipboard.SetText(xe.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public static bool CopyIconPathData(Icon icon)
        {
            try
            {
                Clipboard.SetText(icon.Data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 暂不提供
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static bool CopyIconPng(Icon icon)
        {
            var canvas = GetCanvas(icon);

            var transform = canvas.LayoutTransform;
            canvas.LayoutTransform = null;

            var size = new Size(canvas.Width, canvas.Height);

            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var renderBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);

            canvas.LayoutTransform = transform;
            try
            {
                Clipboard.SetImage(renderBitmap);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public static Canvas GetCanvas(Icon icon)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 32;
            canvas.Height = 32;
            canvas.Background = Brushes.Transparent;

            Grid g = new Grid();
            g.Height = 32;
            g.Width = 32;
            g.Background = Brushes.SeaGreen;

            Path p = new Path();
            p.Data = Geometry.Parse(icon.Data);
            p.Fill = Application.Current.FindResource("WindowText") as SolidColorBrush;
            p.Stretch = Stretch.Uniform;
            p.Margin = new Thickness(3);
            g.Children.Add(p);

            canvas.Children.Add(g);
            //Size z = new Size(canvas.Width, canvas.Height);
            //canvas.Measure(z);
            //canvas.Arrange(new Rect(z));
            return canvas;
        }

        public static bool SavePng(Icon icon, string filename = "")
        {
            if (string.IsNullOrEmpty(filename))
            {
                var d = System.IO.Path.Combine(Environment.CurrentDirectory, "Images");
                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
                filename = System.IO.Path.Combine(d, icon.Name + ".png");
            }

            var canvas = GetCanvas(icon);
            try
            {
                SaveCanvas(canvas, filename);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public static void SaveCanvas(Canvas canvas, string filename)
        {
            Size size = new Size(canvas.Width, canvas.Height);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)canvas.Width, //width 
                (int)canvas.Height, //height 
                96, //dpi x 
                96, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename);
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}
