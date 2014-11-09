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
        /// <summary>
        /// 复制Icon的Path到粘贴板
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 复制Icon的PathData到粘贴板
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
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
        /// 复制Icon为Png到粘贴板
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

        /// <summary>
        /// 通过Icon获取Canvas
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 保存Png文件
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 通过Icon获取XElement
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static XElement GetElementFromIcon(Icon icon)
        {
            var xe = new XElement("item",
                new XAttribute("Name", icon.Name),
                new XAttribute("Width", icon.Width),
                new XAttribute("Height", icon.Height),
                new XAttribute("Data", icon.Data),
                new XAttribute("Keyword", icon.Keyword),
                new XAttribute("CreatedDataTime", icon.CreatedDataTime.ToString("d")),
                new XAttribute("ModifiedDataTime", icon.ModifiedDataTime.ToString("d")),
                new XAttribute("IsFavourite", icon.IsFavourite)
                );
            return xe;
        }

        /// <summary>
        /// 通过XElement获取Icon
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Icon GetIconFromElement(XElement element)
        {
            if (element == null ||
                element.Attribute("Name") == null ||
                element.Attribute("Data") == null ||
                element.Attribute("Height") == null ||
                element.Attribute("Width") == null)
                return null;

            var icon = new Icon();
            icon.Name = element.Attribute("Name").Value;
            icon.Width = Math.Round(double.Parse(element.Attribute("Width").Value));
            icon.Height = Math.Round(double.Parse(element.Attribute("Height").Value));
            icon.Data = element.Attribute("Data").Value;

            if (element.Attribute("IsFavourite") != null)
                icon.IsFavourite = bool.Parse(element.Attribute("IsFavourite").Value);

            if (element.Attribute("CreatedDataTime") != null)
            {
                string cdt = element.Attribute("CreatedDataTime").Value;
                if (!string.IsNullOrEmpty(cdt))
                {
                    DateTime createdDataTime;
                    DateTime.TryParse(cdt, out createdDataTime);
                    icon.CreatedDataTime = createdDataTime;
                }
            }

            if (element.Attribute("ModifiedDataTime") != null)
            {
                string mdt = element.Attribute("ModifiedDataTime").Value;
                if (!string.IsNullOrEmpty(mdt))
                {
                    DateTime modifiedDataTime;
                    DateTime.TryParse(mdt, out modifiedDataTime);
                    icon.ModifiedDataTime = modifiedDataTime;
                }
            }
            if (element.Attribute("Keyword") != null)
                icon.Keyword = element.Attribute("Keyword").Value;

            return icon;
        }

        /// <summary>
        /// 修改XElement
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void ModifyIconElement(XElement target, XElement source)
        {
            if (target == null || source == null)
            {
                return;
            }
            if (target.Attribute("Name") != null && source.Attribute("Name") != null)
            {
                target.Attribute("Name").Value = source.Attribute("Name").Value;
            }
            if (target.Attribute("Width") != null && source.Attribute("Width") != null)
            {
                target.Attribute("Width").Value = source.Attribute("Width").Value;
            }
            if (target.Attribute("Height") != null && source.Attribute("Height") != null)
            {
                target.Attribute("Height").Value = source.Attribute("Height").Value;
            }
            if (target.Attribute("Data") != null && source.Attribute("Data") != null)
            {
                target.Attribute("Data").Value = source.Attribute("Data").Value;
            }
            if (target.Attribute("Keyword") != null && source.Attribute("Keyword") != null)
            {
                target.Attribute("Keyword").Value = source.Attribute("Keyword").Value;
            }
            if (target.Attribute("IsFavourite") != null && source.Attribute("IsFavourite") != null)
            {
                target.Attribute("IsFavourite").Value = source.Attribute("IsFavourite").Value;
            }
            if (target.Attribute("CreatedDateTime") != null && source.Attribute("CreatedDateTime") != null)
            {
                target.Attribute("CreatedDateTime").Value = source.Attribute("CreatedDateTime").Value;
            }
            if (target.Attribute("ModifiedDateTime") != null && source.Attribute("ModifiedDateTime") != null)
            {
                target.Attribute("ModifiedDateTime").Value = source.Attribute("ModifiedDateTime").Value;
            }
        }

        /// <summary>
        /// 修改XElement
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void ModifyIcon(Icon target, Icon source)
        {
            if (target == null || source == null)
            {
                return;
            }

            target.Name = source.Name;
            target.Width = source.Width;
            target.Height = source.Height;
            target.Keyword = source.Keyword;
            target.CreatedDataTime = source.CreatedDataTime;
            target.ModifiedDataTime = source.ModifiedDataTime;
            target.IsFavourite = source.IsFavourite;
        }

        #region Private Method

        /// <summary>
        //  保存RTB到Png
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="filename"></param>
        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }

        /// <summary>
        /// 保存Canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="filename"></param>
        private static void SaveCanvas(Canvas canvas, string filename)
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

        #endregion
    }
}
