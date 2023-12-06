using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FarAway2._0.Tools.Extensions
{
    public static class GenericListExtensions
    {
        public static List<T> SearchData<T>(this List<T> query, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return query;
            }

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType != typeof(ICollection));
            return query.Where(t =>
                properties.Any(p =>
                    p.GetValue(t) != null &&
                    p.GetValue(t).ToString().Contains(text, StringComparison.OrdinalIgnoreCase)
                    )
                ).ToList();
        }
    }
}
