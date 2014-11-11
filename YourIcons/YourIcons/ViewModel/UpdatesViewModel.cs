using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using YourIcons.Model;

namespace YourIcons.ViewModel
{
    public class UpdatesViewModel : ViewModelBase
    {
        private ListCollectionView m_collectionView;
        private IList<Icon> m_updateIconsList;
        private Icon m_selectedIcon;
        private Icon m_firstIcon;

        public Icon SelectedIcon
        {
            get
            {
                return m_selectedIcon;
            }
            set
            {
                if (m_selectedIcon != value)
                {
                    m_selectedIcon = value;
                    OnPropertyChanged(() => this.SelectedIcon);
                }
            }
        }

        public IList<Icon> UpdateIconsList { get { return m_updateIconsList; } }

        public UpdatesViewModel()
        {
            m_firstIcon = DataRetrieved.Instance.IconList.FirstOrDefault();

            var list = DataRetrieved.Instance.IconList.Where(o => o.CreatedDataTime > m_firstIcon.CreatedDataTime);

            m_updateIconsList = new ObservableCollection<Icon>(list);
            m_collectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(UpdateIconsList);
            m_collectionView.GroupDescriptions.Add(new PropertyGroupDescription("CreatedDateTime"));
            DataRetrieved.Instance.IconAdded += Instance_IconAdded;
            DataRetrieved.Instance.IconDeleted += Instance_IconDeleted;

        }

        void Instance_IconDeleted(object sender, IconEventArgs e)
        {
            if (e.Icon.CreatedDataTime > m_firstIcon.CreatedDataTime)
            {
                m_updateIconsList.Remove(e.Icon);
            }
        }

        void Instance_IconAdded(object sender, IconEventArgs e)
        {
            //if (m_firstIcon == null)
            //    m_firstIcon = e.Icon;
            m_updateIconsList.Add(e.Icon);
        }
    }
}
