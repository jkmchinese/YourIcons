using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourIcons.ViewModel;

namespace YourIcons.Model
{
    /// <summary>
    /// 图标基类
    /// </summary>
    public class IconBase : Entity
    {
        /// <summary>
        /// 图标长度
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 图标宽度
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 图标集ID
        /// </summary>
        public string IconsetID { get; set; }
        /// <summary>
        /// 图标集
        /// </summary>
        public Iconset Iconset { get; set; }
        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsFavourite { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
