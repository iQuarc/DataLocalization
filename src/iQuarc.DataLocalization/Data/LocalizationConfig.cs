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
        private static Func<CultureInfo, object> cultureIdentifier = c => c.TwoLetterISOLanguageName;
        static LocalizationConfig()
        {
            CultureProvider = () => Thread.CurrentThread.CurrentUICulture;
        }

        /// <summary>
        /// Registers an entity which identifies the culture used for translation 
        /// </summary>
        /// <typeparam name="TEntity">The entity type which identifies the language</typeparam>
        /// <param name="languageKeyProperty">Expression which indicates the property of the entity which is used for identifying the culture</param>
        public static void RegisterLocalizationEntity<TEntity>(Expression<Func<TEntity, object>> languageKeyProperty)
        {
            LanguageExpression = languageKeyProperty ?? throw new ArgumentNullException(nameof(languageKeyProperty));
            LocalizationType = typeof(TEntity);
        }

        /// <summary>
        /// Maps a <see cref="CultureInfo"/> as input to a key used to identify the culture entity 
        /// </summary>
        /// <param name="cultureKeyMapper"></param>
        public static void RegisterCultureMapper(Func<CultureInfo, object> cultureKeyMapper)
        {
            cultureIdentifier = cultureKeyMapper ?? throw new ArgumentNullException(nameof(cultureKeyMapper));
        }

        public static Func<CultureInfo> CultureProvider { get; private set; }

        /// <summary>
        /// Registers a provider for the <see cref="CultureInfo"/> used during localization
        /// </summary>
        /// <param name="cultureProvider"></param>
        public static void RegisterLocalizationProvider(Func<CultureInfo> cultureProvider)
        {
            CultureProvider = cultureProvider ?? throw new ArgumentNullException(nameof(cultureProvider));
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

        internal static Func<CultureInfo, object> CultureIdentifier => cultureIdentifier;
    }
}