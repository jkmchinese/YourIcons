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
using LWLCX.Framework.Common.Logger;
using ModernUI.Presentation;
using ModernUI.UIHelper;
using Path = System.Windows.Shapes.Path;

namespace YourIcons.Model
{
    public static class IconHelper
    {
        /// <summary>
        /// 日期统一格式
        /// </summary>
        public static string DateTimeStringShortFormat = "yyyy-MM-dd";

        /// <summary>
        /// 复制Icon的Path到粘贴板
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static bool CopyIconPath(Icon icon)
        {
            try
            {
                var xe = new XElement("Path",
                new XAttribute("Name", icon.Name),
                new XAttribute("Width", icon.Width),
                new XAttribute("Height", icon.Height),
                new XAttribute("Data", icon.Data)
                    //new XAttribute("Keyword", icon.Keyword)
                );
                Clipboard.SetText(xe.ToString());
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Error("CopyIconPath has occur exception:" + ex);
                return false;
            }
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
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Error("CopyIconPathData has occur exception:" + ex);
                return false;
            }
        }

        /// <summary>
        /// 通过Icon获取Canvas
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static Canvas GetCanvas(Canvas canvasExport)
        {
            var exportPath = canvasExport.FindChild<Path>();

            Canvas canvas = new Canvas();
            canvas.Width = canvasExport.Width;
            canvas.Height = canvasExport.Height;
            canvas.Background = canvasExport.Background;

            Grid g = new Grid();
            g.Height = canvasExport.Width;
            g.Width = canvasExport.Height;
            g.Background = Brushes.Transparent;

            Path p = new Path();
            p.Width = exportPath.Width;
            p.Height = exportPath.Height;
            p.Data = exportPath.Data;
            p.Fill = exportPath.Fill;
            p.Stretch = Stretch.Uniform;
            p.Margin = new Thickness(canvasExport.Width - exportPath.Width);
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
        /// <param name="canvas"></param>
        /// <param name="icon"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool SavePng(Canvas canvas, Icon icon, string filename = "")
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    var d = System.IO.Path.Combine(Environment.CurrentDirectory, "Export");
                    if (!Directory.Exists(d))
                    {
                        Directory.CreateDirectory(d);
                    }
                    filename = System.IO.Path.Combine(d, icon.Name + ".png");
                }

                var canvasExport = GetCanvas(canvas);
                SaveCanvas(canvasExport, filename);
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Error("SavePng has occur exception:" + ex);
                return false;
            }
        }

        public static bool SavePathFile(XElement canvasElement, Icon icon, string filename = "")
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    var d = System.IO.Path.Combine(Environment.CurrentDirectory, "Export");
                    if (!Directory.Exists(d))
                    {
                        Directory.CreateDirectory(d);
                    }
                    filename = System.IO.Path.Combine(d, icon.Name + ".xaml");
                }
                canvasElement.Save(filename);
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Error("SavePathFile has occur exception:" + ex);
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
                new XAttribute("Keyword", icon.Keyword ?? string.Empty),
                new XAttribute("CreatedTime", icon.CreatedTime.ToString("d")),
                new XAttribute("ModifiedTime", icon.ModifiedTime == DateTime.MinValue ? string.Empty : icon.ModifiedTime.ToString("d")),
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
            {
                LoggingService.Warn("GetIconFromElement failed for is null or some attributes is null");
                return null;
            }

            var icon = new Icon();
            icon.Name = element.Attribute("Name").Value;
            icon.Width = Math.Round(double.Parse(element.Attribute("Width").Value));
            icon.Height = Math.Round(double.Parse(element.Attribute("Height").Value));
            icon.Data = element.Attribute("Data").Value;

            if (element.Attribute("IsFavourite") != null)
                icon.IsFavourite = bool.Parse(element.Attribute("IsFavourite").Value);

            if (element.Attribute("CreatedTime") != null)
            {
                string cdt = element.Attribute("CreatedTime").Value;
                if (!string.IsNullOrEmpty(cdt))
                {
                    DateTime createdDateTime;
                    DateTime.TryParse(cdt, out createdDateTime);
                    icon.CreatedTime = createdDateTime;
                }
            }

            if (element.Attribute("ModifiedTime") != null)
            {
                string mdt = element.Attribute("ModifiedTime").Value;
                if (!string.IsNullOrEmpty(mdt))
                {
                    DateTime modifiedDateTime;
                    DateTime.TryParse(mdt, out modifiedDateTime);
                    icon.ModifiedTime = modifiedDateTime;
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
            if (target.Attribute("CreatedTime") != null && source.Attribute("CreatedTime") != null)
            {
                target.Attribute("CreatedTime").Value = source.Attribute("CreatedTime").Value;
            }
            if (target.Attribute("ModifiedTime") != null && source.Attribute("ModifiedTime") != null)
            {
                target.Attribute("ModifiedTime").Value = source.Attribute("ModifiedTime").Value;
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
            target.CreatedTime = source.CreatedTime;
            target.ModifiedTime = source.ModifiedTime;
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
