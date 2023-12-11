using System.Globalization;
using System.Windows.Data;

namespace FarAway2._0.Converters
{
    class ParkingSpaceRentalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ICollection<ParkingSpaceRental> Rentals = value as ICollection<ParkingSpaceRental>;
            Users user = DbUtils.GetLastUserRental(Rentals);
            return user == null ? null : DbUtils.GetUserInitials(user);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
