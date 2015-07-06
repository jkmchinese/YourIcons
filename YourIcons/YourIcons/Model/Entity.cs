using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourIcons.Model
{
    /// <summary>
    /// 实体
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 实体名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 实体创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 实体修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
    }
}
