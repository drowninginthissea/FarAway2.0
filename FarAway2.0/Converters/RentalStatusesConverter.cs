using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FarAway2._0.Converters
{
    internal class RentalStatusesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RentalStatuses status = value as RentalStatuses;
            switch (status.id)
            {
                case Entities.Enums.RentalStatuses.Active:
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA8, 0xF3));
                case Entities.Enums.RentalStatuses.Completed:
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0x0E, 0xD1, 0x45));
                case Entities.Enums.RentalStatuses.Canceled:
                    return new SolidColorBrush(Color.FromArgb(0xFF, 0xEC, 0x1C, 0x24));
                default:
                    return new SolidColorBrush(Color.FromArgb(0xff, 0xFF, 0x7F, 0x27));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
