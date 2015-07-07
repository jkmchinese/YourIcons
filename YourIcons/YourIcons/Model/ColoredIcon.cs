using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourIcons.Model
{
    /// <summary>
    /// 颜色图标
    /// </summary>
    public class ColoredIcon : Icon
    {
        public IEnumerable<ColoredIconItem> Paths { get; set; }
    }
}
