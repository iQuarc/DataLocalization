using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iQuarc.DataLocalization.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iQuarc.DataLocalization.Tests.UnitTests
{
    [TestClass]
    public class CultureKeyMappingTests
    {
        [ClassInitialize]
        public static void Setup(TestContext ctx)
        {
            LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.IsoCode);
            LocalizationConfig.RegisterLocalizationProvider(DefaultTestCulture);
        }

        static CultureInfo DefaultTestCulture()
        {
            return new CultureInfo("fr");
        }


        [TestMethod]
        public void LanguageEntityIsIdentifiedUsingThreeLetterIsoCode()
        {
            LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.ThreeLetterIsoCode);
            LocalizationConfig.RegisterCultureMapper(c => c.ThreeLetterISOLanguageName);

            var beerCategory = GetCategories()
                .Where(c => c.Id == 1)
                .Select(c => new { ID = c.Id, Name = c.Name })
                .Localize()
                .First();

            Assert.AreEqual("Bières", beerCategory.Name);
        }

        [TestMethod]
        public void LanguageEntityIsIdentifiedLCID()
        {
            LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.LCID);
            LocalizationConfig.RegisterCultureMapper(c => c.LCID);

            var beerCategory = GetCategories()
                .Where(c => c.Id == 1)
                .Select(c => new { ID = c.Id, Name = c.Name })
                .Localize()
                .First();

            Assert.AreEqual("Bières", beerCategory.Name);
        }


        static IQueryable<Category> GetCategories()
        {
            return GetSample().AsQueryable();

            IEnumerable<Category> GetSample()
            {
                var fr = new Language { Id = 1, IsoCode = "fr", ThreeLetterIsoCode = "fra",LCID = 12, Name = "French" };
                var ro = new Language { Id = 2, IsoCode = "ro", ThreeLetterIsoCode = "ron",LCID = 24, Name = "Romanian" };

                yield return new Category
                {
                    Id = 1,
                    Name = "Beers",
                    Localizations = new List<CateogoryLocalization>
                    {
                        new CateogoryLocalization {CategoryId = 1, LanguageId = 1, Language = fr, Name = "Bières"},
                        new CateogoryLocalization {CategoryId = 1, LanguageId = 2, Language = ro, Name = "Beri"}
                    }
                };
                yield return new Category
                {
                    Id = 2,
                    Name = "Wines",
                    Localizations = new List<CateogoryLocalization>
                    {
                        new CateogoryLocalization {CategoryId = 2, LanguageId = 1, Language = fr, Name = "Vins"},
                        new CateogoryLocalization {CategoryId = 2, LanguageId = 2, Language = ro, Name = "Vinuri"}
                    }
                };
                yield return new Category
                {
                    Id = 3,
                    Name = "Foods",
                    Localizations = new List<CateogoryLocalization>
                    {
                        new CateogoryLocalization {CategoryId = 3, LanguageId = 1, Language = fr, Name = "Aliments"},
                    }
                };
            }
        }
    }
}