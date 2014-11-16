using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LWLCX.Framework.Common.Logger;

namespace YourIcons.Model
{
    public class DataRetrieved
    {
        private const string APPDATA = @"App_Data";
        private const string FILEEXTEND = ".xml";
        private static object _locker = new object();
        private static DataRetrieved _instance;
        private readonly IList<Icon> m_iconLists;
        private IDictionary<XElement, string> m_doc_file;
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
            m_doc_file = new Dictionary<XElement, string>();
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                LoggingService.Error("DataRetrieved LoadData occur exception:\r\n" + ex);
            }
        }

        public IList<Icon> IconList { get { return m_iconLists; } }

        public bool AddIcon(Icon icon)
        {
            LoggingService.Debug("start to addIcon");
            if (!ValidateIcon(icon))
            {
                LoggingService.Warn("addIcon failed, Icon name is Duplicated:" + icon.Name);
                return false;
            }

            var addElement = IconHelper.GetElementFromIcon(icon);
            var saveElement = GetSaveElement(icon);
            bool result = SaveData(saveElement, addElement);
            if (result)
            {
                var newIcon = icon.Clone() as Icon;
                m_iconLists.Add(newIcon);
                OnIconAdded(newIcon);
            }
            LoggingService.Debug("end to addIcon,result:" + result);
            return result;
        }

        public bool FavoriteIcon(Icon icon)
        {
            if (icon.IsFavourite)
            {
                return true;
            }
            LoggingService.Debug("start to FavoriteIcon:" + icon.Name);
            icon.IsFavourite = true;
            var result = ModifiedIcon(icon);
            LoggingService.Debug("end to FavoriteIcon,result:" + result);
            return result;
        }

        public bool UnFavoriteIcon(Icon icon)
        {
            if (!icon.IsFavourite)
            {
                return true;
            }
            LoggingService.Debug("start to UnFavoriteIcon:" + icon.Name);
            icon.IsFavourite = false;
            var result = ModifiedIcon(icon);
            LoggingService.Debug("end to UnFavoriteIcon,result:" + result);
            return true;
        }

        private XElement GetSaveElement(Icon icon, bool isExit = false)
        {
            string dateTime = icon.CreatedDataTime.ToString(IconHelper.DateTimeStringShortFormat);

            foreach (KeyValuePair<XElement, string> keyValuePair in m_doc_file)
            {
                if (keyValuePair.Value.Contains(dateTime))
                {
                    return keyValuePair.Key;
                }
            }

            if (isExit)
            {
                return m_doc_file.FirstOrDefault().Key;
            }

            //不存在 则新建
            var newXElement = new XElement("root");
            string filePath = Path.Combine(Environment.CurrentDirectory, @"App_Data\Data_" + dateTime + FILEEXTEND);
            newXElement.Save(filePath);
            m_doc_file.Add(newXElement, Path.GetFileName(filePath));
            return newXElement;
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
            LoggingService.Debug("start to modifiedIcon");
            if (icon == null)
            {
                LoggingService.Warn("ModifiedIcon:icon is null");
                return false;
            }
            var localIcon = m_iconLists.FirstOrDefault(o => o.Name == icon.Name);
            if (localIcon == null)
            {
                LoggingService.Warn("ModifiedIcon:could not find the icon to modify");
                return false;
            }
            XElement x = IconHelper.GetElementFromIcon(icon);
            var saveElement = GetSaveElement(icon, true);
            bool result = ModifiedData(saveElement, x);
            if (result)
            {
                IconHelper.ModifyIcon(localIcon, icon);
                OnIconModified(localIcon);
            }
            LoggingService.Debug("end to modifiedIcon,result:" + result);
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
            LoggingService.Debug("start to deleteIcon");
            if (icon == null)
            {
                LoggingService.Warn("DeleteIcon:icon is null");
                return false;
            }
            var localIcon = m_iconLists.FirstOrDefault(o => o.Name == icon.Name);
            if (localIcon == null)
            {
                LoggingService.Warn("DeleteIcon:could note find the icon to delete");
                return false;
            }
            var deleteElement = IconHelper.GetElementFromIcon(icon);
            var saveElement = GetSaveElement(icon, true);
            bool result = DeleteData(saveElement, deleteElement);
            if (result)
            {
                m_iconLists.Remove(localIcon);
                OnIconDeleted(localIcon);
            }
            LoggingService.Debug("end to deleteIcon,result:" + result);
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
            LoggingService.Debug("start to batchAddIcon");
            if (iconList == null)
            {
                LoggingService.Warn("batchAddIcon:iconList is null");
                return false;
            }
            var icons = new List<Icon>();
            foreach (Icon icon in iconList)
            {
                if (ValidateIcon(icon))
                {
                    icons.Add(icon);
                }
                else
                {
                    LoggingService.Warn("BatchAddIcon:ValidateIcon failed:name=" + icon.Name);
                }
            }
            if (icons.Count <= 0)
            {
                LoggingService.Warn("batchAddIcon:iconList is empty");
                return false;
            }

            var addElementList = icons.Select(IconHelper.GetElementFromIcon).ToList();
            var saveElement = GetSaveElement(icons.FirstOrDefault());
            bool result = SaveData(saveElement, addElementList);

            if (result)
            {
                foreach (var icon in icons)
                {
                    var newIcon = icon.Clone() as Icon;
                    m_iconLists.Add(newIcon);
                    OnIconAdded(newIcon);
                }
            }
            LoggingService.Debug("end to batchAddIcon,result:" + result);
            return result;
        }

        private void LoadData()
        {
            LoggingService.Info("start to load icons.");
            string path = Path.Combine(System.Environment.CurrentDirectory, APPDATA);
            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file) == FILEEXTEND)
                {
                    LoadFile(file);
                }
            }
            LoggingService.Info("end to load icons.icons count:" + m_iconLists.Count);
        }

        private void LoadFile(string file)
        {
            int count = 0;
            var doc = XElement.Load(file);
            foreach (var xn in doc.Elements())
            {
                var icon = IconHelper.GetIconFromElement(xn);
                if (icon != null)
                {
                    count++;
                    m_iconLists.Add(icon);
                }
                else
                {
                    LoggingService.Warn("LoadFile has icon error:file" + file +
                        "element:" + xn);
                }
            }
            LoggingService.Debug("loaded file:" + file + ",add icons:" + count);
            m_doc_file.Add(doc, Path.GetFileName(file));
        }

        private string GetFilePath(XElement doc)
        {
            return Path.Combine(Environment.CurrentDirectory, APPDATA, m_doc_file[doc]);
        }

        private bool SaveData(XElement doc, XElement element)
        {
            try
            {
                doc.Add(element);
                doc.Save(GetFilePath(doc));
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Fatal("SaveData(single) has occur exception:" + ex);
                return false;
            }
        }

        private bool ModifiedData(XElement doc, XElement element)
        {
            try
            {
                var el = doc.Elements().FirstOrDefault(o => o.Attribute("Name").Value == element.Attribute("Name").Value);
                if (el != null)
                {
                    IconHelper.ModifyIconElement(el, element);
                }
                doc.Save(GetFilePath(doc));
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Fatal("ModifiedData has occur exception:" + ex);
                return false;
            }
        }

        private bool DeleteData(XElement doc, XElement element)
        {
            try
            {
                var el = doc.Elements().FirstOrDefault(o => o.Attribute("Name").Value == element.Attribute("Name").Value);
                if (el != null)
                {
                    el.Remove();
                }
                doc.Save(GetFilePath(doc));
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Fatal("DeleteData has occur exception:" + ex);
                return false;
            }
        }

        private bool SaveData(XElement doc, IEnumerable<XElement> elements)
        {
            try
            {
                foreach (var element in elements)
                {
                    if (element != null)
                    {
                        doc.Add(element);
                    }
                }
                doc.Save(GetFilePath(doc));
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.Fatal("SaveData(multi) has occur exception:" + ex);
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

        public bool ValidateIconName(string iconName)
        {
            if (string.IsNullOrEmpty(iconName))
            {
                return false;
            }
            return m_iconLists.All(o => o.Name != iconName);
        }

        public event EventHandler<IconEventArgs> IconAdded;
        public event EventHandler<IconEventArgs> IconDeleted;
        public event EventHandler<IconEventArgs> IconModified;
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
