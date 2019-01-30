using System.Globalization;
using System.Linq;

namespace iQuarc.DataLocalization
{
    public static class LocalizedQueryExtensions
    {
        public static ILocalizedQueryable<T> Localize<T>(this IQueryable<T> query)
        {
            return query.Localize(LocalizationConfig.CultureProvider());
        }

        public static ILocalizedQueryable<T> Localize<T>(this IQueryable<T> query, CultureInfo culture)
        {
            return new LocalizedQueryable<T>(query, culture);
        }

        public static ILocalizedQueryable Localize(this IQueryable query)
        {
            return query.Localize(LocalizationConfig.CultureProvider());
        }

        public static ILocalizedQueryable Localize(this IQueryable query, CultureInfo culture)
        {
            return new LocalizedQueryable(query, culture);
        }

    }
}