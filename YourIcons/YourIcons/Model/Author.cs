using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace YourIcons.Model
{
    /// <summary>
    /// 作者
    /// </summary>
    public class Author
    {
        public Author()
        {
            Name = "Transfen";
            Website = "http://www.stefan.top/";
            License = "MIT License";
        }

        /// <summary>
        /// 作者名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 作者网站
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// 完善人
        /// </summary>
        public string Prefect { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// License
        /// </summary>
        public string License { get; set; }

        //static Author()
        //{
        //    LicenseList = new List<string>()
        //    {
        //        "None",
        //        "Apache License 2.0",
        //        "GNU General Public License 2.0",
        //        "MIT License",
        //    };
        //}
        //public static IEnumerable<string> LicenseList { get; private set; }
    }
}
