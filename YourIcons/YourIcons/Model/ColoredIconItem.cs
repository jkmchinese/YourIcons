using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace YourIcons.Model
{
    /// <summary>
    /// 彩色图标项
    /// </summary>
    public class ColoredIconItem
    {
        public string Data { get; set; }
        public double Opacity { get; set; }
        public string Fill { get; set; }
        public string Stroke { get; set; }
        public double StrokeThickness { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
    }
}
