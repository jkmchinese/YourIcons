using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourIcons.ViewModel;

namespace YourIcons.Model
{
    public class IconBase : ViewModelBase, ICloneable
    {
        private string m_name;
        private string m_data;
        private double m_width;
        private double m_height;
        private string m_keyword;
        private const double TOLERANCE = 0.001;
        private DateTime m_createdDateTime;
        private DateTime m_modifiedDateTime;
        private bool m_isFavourite;


        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                if (m_name != value)
                {
                    m_name = value;
                    OnPropertyChanged(() => this.Name);
                }
            }

        }
        public string Keyword
        {
            get
            {
                return m_keyword;
            }
            set
            {
                if (m_keyword != value)
                {
                    m_keyword = value;
                    OnPropertyChanged(() => this.Keyword);
                }
            }

        }
        public string Data
        {
            get
            {
                return m_data;
            }
            set
            {
                if (m_data != value)
                {
                    m_data = value;
                    OnPropertyChanged(() => this.Data);
                }
            }

        }
        public double Width
        {
            get
            {
                return m_width;
            }
            set
            {
                if (Math.Abs(m_width - value) > TOLERANCE)
                {
                    m_width = value;
                    OnPropertyChanged(() => this.Width);
                }
            }

        }
        public double Height
        {
            get
            {
                return m_height;
            }
            set
            {
                if (Math.Abs(m_height - value) > TOLERANCE)
                {
                    m_height = value;
                    OnPropertyChanged(() => this.Height);
                }
            }

        }
        public DateTime CreatedDateTime
        {
            get
            {
                return m_createdDateTime;
            }
            set
            {
                if (m_createdDateTime != value)
                {
                    m_createdDateTime = value;
                    OnPropertyChanged(() => this.CreatedDateTime);
                }
            }
        }
        public DateTime ModifiedDateTime
        {
            get
            {
                return m_modifiedDateTime;
            }
            set
            {
                if (m_modifiedDateTime != value)
                {
                    m_modifiedDateTime = value;
                    OnPropertyChanged(() => this.ModifiedDateTime);
                }
            }
        }
        public bool IsFavourite
        {
            get
            {
                return m_isFavourite;
            }
            set
            {
                if (m_isFavourite != value)
                {
                    m_isFavourite = value;
                    OnPropertyChanged(() => this.IsFavourite);
                }
            }
        }
        public string GroupName { get { return m_createdDateTime.ToString(IconHelper.DateTimeStringShortFormat); } }


        #region ICloneable Members

        public virtual object Clone()
        {
            return new object();
        }

        #endregion
    }
}
