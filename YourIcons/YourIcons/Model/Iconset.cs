using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourIcons.Model
{
    /// <summary>
    /// 图标集
    /// </summary>
    public class Iconset : Entity
    {
        #region Fields

        private readonly IList<IconBase> m_icons;

        #endregion

        #region Constructors

        private static readonly Random _random = new Random();

        public Iconset()
        {
        }

        public Iconset(IList<Icon> icons)
        {
            m_icons = new ObservableCollection<IconBase>();
            int count = icons.Count;
            for (int i = 0; i < 50; i++)
            {
                m_icons.Add(icons[_random.Next(0, count - 1)]);
            }
        }


        #endregion

        #region Properties

        public IEnumerable<IconBase> Icons
        {
            get { return m_icons; }
        }

        public IEnumerable<IconBase> DisplayIcons
        {
            get { return m_icons.Take(16); }
        }

        public Author Author { get; set; }

        public bool Outlined { get; set; }

        public bool Filled { get; set; }

        public bool Colored { get; set; }

        public bool Vector { get; set; }

        #endregion

        #region Events

        #endregion

        #region Private Methods

        #endregion

        #region Protect Methods

        #endregion

        #region Public Methods

        public bool AddIcon(IconBase icon)
        {
            m_icons.Add(icon);
            return true;
        }

        #endregion
    }
}
