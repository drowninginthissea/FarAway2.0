using System.Globalization;
using System.Windows.Data;

namespace FarAway2._0.Converters
{
    public class AdditionalServicesConverter : IValueConverter
    {
        public static string GetAddedServices(ICollection<AdditionalServicesForRent> Services)
        {
            string Result = "";
            foreach (AdditionalServicesForRent service in Services)
            {
                Result += $"{service.idServiceNavigation.ServiceName} " +
                    $"({service.FrequencyOfServicePerformanceInDaysNavigation.FrequencyName})";
                if (service != Services.Last())
                    Result += '\n';
            }
            return Result == "" ? null : Result;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetAddedServices(value as ICollection<AdditionalServicesForRent>);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
