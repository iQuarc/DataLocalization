using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iQuarc.DataLocalization.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iQuarc.DataLocalization.Tests.UnitTests
{
    [TestClass]
    public class QueryTranslationTests
    {
        [ClassInitialize]
        public static void Setup(TestContext ctx)
        {
            LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.IsoCode);
            LocalizationConfig.RegisterLocalizationProvider(DefaultTestCulture);
            LocalizationConfig.RegisterCultureMapper(c => c.TwoLetterISOLanguageName);
        }

        [TestMethod]
        public void TranslateWithDefaultCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories()
                .Where(c => c.Id == 1)
                .Select(c => new {ID = c.Id, Name = c.Name})
                .Localize()
                .First();

            Assert.AreEqual("Bières", beerCategory.Name);
        }

        [TestMethod]
        public void TranslateWifhExplicitCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories()
                .Where(c => c.Id == 1)
                .Select(c => new { ID = c.Id, c.Name })
                .Localize(new CultureInfo("ro-RO"))
                .First();

            Assert.AreEqual("Beri", beerCategory.Name);
        }


        [TestMethod]
        public void TranslateGetsFallbackTranslation()
        {
            var foodCateogory = GetCategories()
                .Where(c => c.Id == 3)
                .Select(c => new { ID = c.Id, c.Name })
                .Localize(new CultureInfo("ro-RO"))
                .First();

            Assert.AreEqual("Foods", foodCateogory.Name);
        }

        [TestMethod]
        public void TranslateGetsFallbackTranslationWithDirectPropertyProjection()
        {
            var foodCateogory = GetCategories()
                .Where(c => c.Id == 3)
                .Select(c => c.Name)
                .Localize(new CultureInfo("ro-RO"))
                .First();

            Assert.AreEqual("Foods", foodCateogory);
        }

        [TestMethod]
        public void TranslateWorksWithDirectPropertyProjection()
        {
            var foodCateogory = GetCategories()
                .Select(c => c.Name)
                .Localize()
                .ToList();

            Assert.AreEqual("Bières", foodCateogory[0]);
            Assert.AreEqual("Vins", foodCateogory[1]);
            Assert.AreEqual("Aliments", foodCateogory[2]);
        }


        [TestMethod]
        public void TranslateDtoWifhDefaultCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories()
                .Where(c => c.Id == 1)
                .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
                .Localize()
                .First();

            Assert.AreEqual("Bières", beerCategory.Name);
        }


        [TestMethod]
        public void TranslateDtoWithConstructorWifhDefaultCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories()
                .Select(c => new CategoryDto(c.Id, c.Name))
                .Localize()
                .ToList();

            Assert.AreEqual("Bières", beerCategory[0].Name);
            Assert.AreEqual("Vins", beerCategory[1].Name);
            Assert.AreEqual("Aliments", beerCategory[2].Name);
        }


        static CultureInfo DefaultTestCulture()
        {
            return new CultureInfo("fr-FR");
        }

        static IQueryable<Category> GetCategories()
        {
            return GetSample().AsQueryable();

            IEnumerable<Category> GetSample()
            {
                var fr = new Language { Id = 1, IsoCode = "fr", Name = "French" };
                var ro = new Language { Id = 2, IsoCode = "ro", Name = "Romanian" };

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

    public class CategoryDto
    {
        public CategoryDto()
        {
            

        }
        public CategoryDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}