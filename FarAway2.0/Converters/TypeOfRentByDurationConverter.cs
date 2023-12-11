using System.Globalization;
using System.Windows.Data;

namespace FarAway2._0.Converters
{
    public class TypeOfRentByDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TypeOfRentByDuration type = (TypeOfRentByDuration)value;
            string DaysCount = type.MaxDurationOfRentalDays == null ? $"от 31 дня" :
                $"от {type.MinDurationOfRentalDays} до {type.MaxDurationOfRentalDays} дней";
            return $"{type.TypeName}({DaysCount})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
