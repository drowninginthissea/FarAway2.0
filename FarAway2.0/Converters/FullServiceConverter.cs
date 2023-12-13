using System.Globalization;
using System.Windows.Data;

namespace FarAway2._0.Converters
{
    public class FullServiceConverter : IValueConverter
    {
        public static string ConvertService(ListOfAdditionalServices Service)
        {
            return $"{Service.ServiceName}\n{Service.ServicePrice:0.00} ₽";
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertService(value as ListOfAdditionalServices);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
