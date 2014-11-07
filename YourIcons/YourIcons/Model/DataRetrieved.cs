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
            XElement x = GetElementFromIcon(icon);
            bool result = SaveData(x);
            if (result)
            {
                m_iconLists.Add(icon.Clone() as Icon);
            }
            return result;
        }

        public bool ModifiedIcon(Icon icon)
        {
            var localIcon = m_iconLists.FirstOrDefault(o => o.Name == icon.Name);
            if (localIcon == null)
            {
                return false;
            }
            XElement x = GetElementFromIcon(icon);
            return ModifiedData(x);
        }

        public bool DeleteIcon(Icon icon)
        {
            var localIcon = m_iconLists.FirstOrDefault(o => o.Name == icon.Name);
            if (localIcon == null)
            {
                return false;
            }
            XElement x = GetElementFromIcon(icon);
            bool result = DeleteData(x);
            if (result)
            {
                m_iconLists.Remove(localIcon);
            }
            return result;
        }

        public bool BatchAddIcon(IEnumerable<Icon> iconList)
        {
            var xList = IconList.Select(GetElementFromIcon).ToList();
            bool result = SaveData(xList);
            if (result)
            {
                foreach (var icon in iconList)
                {
                    m_iconLists.Add(icon.Clone() as Icon);
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
                var icon = GetIconFromElement(xn);
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
                    ModifyIconElement(el, element);
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

        private void ModifyIconElement(XElement target, XElement source)
        {
            target.Attribute("Name").Value = source.Attribute("Name").Value;
            target.Attribute("Width").Value = source.Attribute("Width").Value;
            target.Attribute("Height").Value = source.Attribute("Height").Value;
            target.Attribute("Data").Value = source.Attribute("Data").Value;
            target.Attribute("Keyword").Value = source.Attribute("Keyword").Value;
        }

        private bool SaveData(IEnumerable<XElement> elements)
        {
            try
            {
                foreach (var element in elements)
                {
                    m_doc.Add(element);
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

        public XElement GetElementFromIcon(Icon icon)
        {
            var xe = new XElement("item",
                new XAttribute("Name", icon.Name),
                new XAttribute("Width", icon.Width),
                new XAttribute("Height", icon.Height),
                new XAttribute("Data", icon.Data),
                new XAttribute("Keyword", icon.Keyword)
                );
            return xe;
        }

        public Icon GetIconFromElement(XElement element)
        {
            if (element == null ||
                element.Attribute("Name") == null ||
                element.Attribute("Data") == null ||
                element.Attribute("Height") == null ||
                element.Attribute("Width") == null)
                return null;
            var icon = new Icon();
            icon.Name = element.Attribute("Name").Value;
            icon.Width = Math.Round(double.Parse(element.Attribute("Width").Value));
            icon.Height = Math.Round(double.Parse(element.Attribute("Height").Value));
            icon.Data = element.Attribute("Data").Value;

            if (element.Attribute("Keyword") != null)
                icon.Keyword = element.Attribute("Keyword").Value;

            return icon;
        }

    }
}
