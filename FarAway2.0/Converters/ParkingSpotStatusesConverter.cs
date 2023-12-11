using FarAway2._0.Entities;
using FarAway2._0.Entities.Enums;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FarAway2._0.Converters
{
    class ParkingSpotsStatusesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Entities.ParkingSpotStatuses status = value as Entities.ParkingSpotStatuses;
            switch (status.id)
            {
                case Entities.Enums.ParkingSpotStatuses.Free:
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x0E, 0xD1, 0x45));
                case Entities.Enums.ParkingSpotStatuses.Busy:
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0xEC, 0x1C, 0x24));
                case Entities.Enums.ParkingSpotStatuses.OnServing:
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA8, 0xF3));
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
