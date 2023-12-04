using System.Globalization;
using System.Windows.Data;

namespace FarAway2._0.Converters
{
    internal class PhoneNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string phoneNumber && phoneNumber.Length == 11)
            {
                return $"+{phoneNumber.Substring(0, 1)}({phoneNumber.Substring(1, 3)}){phoneNumber.Substring(4, 3)}-{phoneNumber.Substring(7, 2)}-{phoneNumber.Substring(9)}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string formattedPhoneNumber)
            {
                return formattedPhoneNumber.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "");
            }
            return value;
        }
    }
}
