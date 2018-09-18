using System.Collections.Generic;
using System.Linq;
using iQuarc.DataLocalization.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iQuarc.DataLocalization.Tests.UnitTests
{
    [TestClass]
    public class QueryTranslationTestsInMemory : QueryTranslationTestsBase
    {

        [ClassInitialize]
        public static void Setup(TestContext ctx)
        {
            LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.IsoCode);
            LocalizationConfig.RegisterLocalizationProvider(DefaultTestCulture);
            LocalizationConfig.RegisterCultureMapper(c => c.TwoLetterISOLanguageName);
        }

        protected override IQueryable<Category> GetCategories()
        {
            return GetSample().AsQueryable();

            IEnumerable<Category> GetSample()
            {
                var fr = new Language {Id = 1, IsoCode = "fr", Name = "French"};
                var ro = new Language {Id = 2, IsoCode = "ro", Name = "Romanian"};

                yield return new Category
                {
                    Id   = 1,
                    Name = "Beers",
                    
                    Localizations = new List<CategoryLocalization>
                    {
                        new CategoryLocalization {CategoryId = 1, LanguageId = 1, Language = fr, Name = "Bières"},
                        new CategoryLocalization {CategoryId = 1, LanguageId = 2, Language = ro, Name = "Beri"}
                    }
                };
                yield return new Category
                {
                    Id   = 2,
                    Name = "Wines",
                    Localizations = new List<CategoryLocalization>
                    {
                        new CategoryLocalization {CategoryId = 2, LanguageId = 1, Language = fr, Name = "Vins"},
                        new CategoryLocalization {CategoryId = 2, LanguageId = 2, Language = ro, Name = "Vinuri"}
                    }
                };
                yield return new Category
                {
                    Id   = 3,
                    Name = "Foods",
                    Localizations = new List<CategoryLocalization>
                    {
                        new CategoryLocalization {CategoryId = 3, LanguageId = 1, Language = fr, Name = "Aliments"}
                    }
                };
            }
        }
    }
}