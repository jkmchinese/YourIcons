using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourIcons.ViewModel
{
    class ViewModelRetrived
    {
        private static object _locker = new object();
        private static ViewModelRetrived _instance;
        private NavViewModel m_navViewModelInstance;
        private IconsViewModel m_iconsViewModelInstance;
        private IconsetViewModel m_iconsetViewModelInstance;
        private NewIconsViewModel m_newIconsViewModelInstance;

        public static ViewModelRetrived Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                            _instance = new ViewModelRetrived();
                    }
                }
                return _instance;
            }
        }
        private ViewModelRetrived()
        {

        }

        public NavViewModel NavViewModelInstance
        {
            get
            {
                if (m_navViewModelInstance == null)
                {
                    lock (_locker)
                    {
                        if (m_navViewModelInstance == null)
                            m_navViewModelInstance = new NavViewModel();
                    }
                }
                return m_navViewModelInstance;
            }
        }

        public IconsViewModel IconsViewModelInstance
        {
            get
            {
                if (m_iconsViewModelInstance == null)
                {
                    lock (_locker)
                    {
                        if (m_iconsViewModelInstance == null)
                            m_iconsViewModelInstance = new IconsViewModel();
                    }
                }
                return m_iconsViewModelInstance;
            }
        }

        public NewIconsViewModel NewIconsViewModelInstance
        {
            get
            {
                if (m_newIconsViewModelInstance == null)
                {
                    lock (_locker)
                    {
                        if (m_newIconsViewModelInstance == null)
                            m_newIconsViewModelInstance = new NewIconsViewModel();
                    }
                }
                return m_newIconsViewModelInstance;
            }
        }

        public IconsetViewModel IconsetViewModelInstance
        {
            get
            {
                if (m_iconsetViewModelInstance == null)
                {
                    lock (_locker)
                    {
                        if (m_iconsetViewModelInstance == null)
                            m_iconsetViewModelInstance = new IconsetViewModel();
                    }
                }
                return m_iconsetViewModelInstance;
            }
        }

    }
}
