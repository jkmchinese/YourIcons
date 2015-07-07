using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourIcons.Model
{
    /// <summary>
    /// 基本图标
    /// </summary>
    public class Icon : IconBase
    {
        /// <summary>
        /// 填充路径数据
        /// </summary>
        public string FilledData { get; set; }
        /// <summary>
        /// 线条路径数据
        /// </summary>
        public string OutlinedData { get; set; }
    }
}
