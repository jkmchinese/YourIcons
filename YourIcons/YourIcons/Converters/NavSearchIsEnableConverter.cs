using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace YourIcons.Converters
{
    public class NavSearchIsEnableConverter : IValueConverter
    {
        private const string ICONSVIEWURI = @"/View/IconsView.xaml";

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri uri = value as Uri;
            if (uri == null)
            {
                return false;
            }

            if (uri.ToString() == ICONSVIEWURI)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
