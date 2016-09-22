using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;

namespace iQuarc.DataLocalization
{
    public static class LocalizationConfig
    {
        private static Expression _languageExpression;
        private static Type _localizationType;

        static LocalizationConfig()
        {
            CultureProvider = () => Thread.CurrentThread.CurrentUICulture;
        }

        public static void RegisterLocalizationEntity<T>(Expression<Func<T, string>> twoLeterISOCodeProperty)
        {
            if (twoLeterISOCodeProperty == null) throw new ArgumentNullException(nameof(twoLeterISOCodeProperty));
            LanguageExpression = twoLeterISOCodeProperty;
            LocalizationType = typeof(T);
        }

        public static Func<CultureInfo> CultureProvider { get; private set; }

        public static void RegisterLocalizationProvider(Func<CultureInfo> cultureProvider)
        {
            if (cultureProvider == null)
                throw new ArgumentNullException(nameof(cultureProvider));
            CultureProvider = cultureProvider;
        }

        internal static Expression LanguageExpression
        {
            get
            {
                if (_languageExpression == null)
                    throw new InvalidOperationException($"Localization language property not registered. Use {nameof(LocalizationConfig)}.{nameof(RegisterLocalizationEntity)} method prior to making any localized queries");
                return _languageExpression; 
            }
            private set { _languageExpression = value; }
        }

        internal static Type LocalizationType
        {
            get
            {
                if (_localizationType == null)
                    throw new InvalidOperationException($"Localization language property not registered. Use {nameof(LocalizationConfig)}.{nameof(RegisterLocalizationEntity)} method prior to making any localized queries");
                return _localizationType;
            }
            private set { _localizationType = value; }
        }
    }
}