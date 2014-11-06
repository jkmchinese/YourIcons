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
    }
}
