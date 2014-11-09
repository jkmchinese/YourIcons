using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace YourIcons.Model
{
    public class DataRetrieved
    {
        private static object _locker = new object();
        private static DataRetrieved _instance;
        private readonly IList<Icon> m_iconLists;
        private XElement m_doc;
        private string m_filePath;
        public static DataRetrieved Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                            _instance = new DataRetrieved();
                    }
                }
                return _instance;
            }
        }

        private DataRetrieved()
        {
            m_iconLists = new ObservableCollection<Icon>();
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public IList<Icon> IconList { get { return m_iconLists; } }

        public bool AddIcon(Icon icon)
        {
            if (!ValidateIcon(icon))
            {
                Debug.WriteLine("AddIcon failed,Icon name is Duplicated:" + icon.Name);
                return false;
            }

            XElement x = IconHelper.GetElementFromIcon(icon);
            bool result = SaveData(x);
            if (result)
            {
                var newIcon = icon.Clone() as Icon;
                m_iconLists.Add(newIcon);
                OnIconAdded(newIcon);
            }
            return result;
        }

        private void OnIconAdded(Icon newIcon)
        {
            if (IconAdded != null)
            {
                IconAdded(this, new IconEventArgs(newIcon));
            }
        }

        public bool ModifiedIcon(Icon icon)
        {
            if (icon == null)
            {
                return false;
            }
            var localIcon = m_iconLists.FirstOrDefault(o => o.Name == icon.Name);
            if (localIcon == null)
            {
                return false;
            }
            XElement x = IconHelper.GetElementFromIcon(icon);
            bool result = ModifiedData(x);
            if (result)
            {
                IconHelper.ModifyIcon(localIcon, icon);
                OnIconModified(localIcon);
            }
            return result;
        }

        private void OnIconModified(Icon icon)
        {
            if (IconModified != null)
            {
                IconModified(this, new IconEventArgs(icon));
            }
        }

        public bool DeleteIcon(Icon icon)
        {
            if (icon == null)
            {
                return false;
            }
            var localIcon = m_iconLists.FirstOrDefault(o => o.Name == icon.Name);
            if (localIcon == null)
            {
                return false;
            }
            XElement x = IconHelper.GetElementFromIcon(icon);
            bool result = DeleteData(x);
            if (result)
            {
                m_iconLists.Remove(localIcon);
                OnIconDeleted(localIcon);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="icon"></param>
        private void OnIconDeleted(Icon icon)
        {
            if (IconDeleted != null)
            {
                IconDeleted(this, new IconEventArgs(icon));
            }
        }

        /// <summary>
        /// 批量新建图标
        /// </summary>
        /// <param name="iconList"></param>
        /// <returns></returns>
        public bool BatchAddIcon(IEnumerable<Icon> iconList)
        {
            if (iconList == null)
            {
                Debug.WriteLine("BatchAddIcon:iconList is null");
                return false;
            }
            var enumerable = iconList as IList<Icon> ?? iconList.ToList();
            var xList = enumerable.Select(o =>
            {
                if (ValidateIcon(o))
                {
                    return IconHelper.GetElementFromIcon(o);
                }
                Debug.WriteLine("BatchAddIcon failed,Icon name is Duplicated:" + o.Name);
                return null;
            }).ToList();

            bool result = SaveData(xList);
            if (result)
            {
                foreach (var icon in enumerable)
                {
                    var newIcon = icon.Clone() as Icon;
                    m_iconLists.Add(newIcon);
                    OnIconAdded(newIcon);
                }
            }
            return result;
        }

        private void LoadData()
        {
            m_filePath = Path.Combine(System.Environment.CurrentDirectory, @"App_Data\Data.xml");
            m_doc = XElement.Load(m_filePath);
            foreach (var xn in m_doc.Elements())
            {
                var icon = IconHelper.GetIconFromElement(xn);
                if (icon != null)
                {
                    m_iconLists.Add(icon);
                }
                else
                {
                    Debug.WriteLine("LoadData has icon error:" + xn);
                }
            }
        }

        private bool SaveData(XElement element)
        {
            try
            {
                m_doc.Add(element);
                m_doc.Save(m_filePath);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        private bool ModifiedData(XElement element)
        {
            try
            {
                var el = m_doc.Elements().FirstOrDefault(o => o.Attribute("Name").Value == element.Attribute("Name").Value);
                if (el != null)
                {
                    IconHelper.ModifyIconElement(el, element);
                }
                m_doc.Save(m_filePath);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        private bool DeleteData(XElement element)
        {
            try
            {
                var el = m_doc.Elements().FirstOrDefault(o => o.Attribute("Name").Value == element.Attribute("Name").Value);
                if (el != null)
                {
                    el.Remove();
                }
                m_doc.Save(m_filePath);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        private bool SaveData(IEnumerable<XElement> elements)
        {
            try
            {
                foreach (var element in elements)
                {
                    if (element != null)
                    {
                        m_doc.Add(element);
                    }
                }
                m_doc.Save(m_filePath);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 如果列表中存在同名的Icon 则返回False 反之则返回True
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        private bool ValidateIcon(Icon icon)
        {
            return m_iconLists.All(o => o.Name != icon.Name);
        }

        public EventHandler<IconEventArgs> IconAdded;
        public EventHandler<IconEventArgs> IconDeleted;
        public EventHandler<IconEventArgs> IconModified;
    }

    /// <summary>
    /// 图标事件参数
    /// </summary>
    public class IconEventArgs : EventArgs
    {
        public Icon Icon { get; set; }

        public IconEventArgs(Icon icon)
        {
            Icon = icon;
        }
    }
}
