using System.Globalization;
using System.Windows.Data;

namespace FarAway2._0.Converters
{
    public class UserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Users users = value as Users;
            return DbUtils.GetUserInitials(users);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
