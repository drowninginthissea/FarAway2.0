using System.Globalization;
using System.Windows.Data;

namespace FarAway2._0.Converters
{
    public class AddedServicesRentalConverter : IValueConverter
    {
        public static string ConvertRental(ParkingSpaceRental Rental)
        {
            return $"{DbUtils.GetUserInitials(Rental.idUserNavigation)}\n" +
                $"{Rental.RentalStartDate:dd.MM.yyyy} - " +
                $"{(Rental.RentEndDate == null ? "Настоящее время" : $"{Rental.RentEndDate:dd.MM.yyyy}")}\n" +
                $"{Rental.idParkingSpotNavigation.idBranchNavigation.Address}";
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertRental(value as ParkingSpaceRental);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
