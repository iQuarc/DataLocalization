using System.Globalization;
using System.Linq;
using iQuarc.DataLocalization.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iQuarc.DataLocalization.Tests.UnitTests
{
    public abstract class QueryTranslationTestsBase
    {
        [TestMethod]
        public void TranslateDtoWifhDefaultCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories()
                .Where(c => c.Id == 1)
                .Select(c => new CategoryDto {Id = c.Id, Name = c.Name, Description = c.Description})
                .Localize()
                .First();

            Assert.AreEqual("Bières", beerCategory.Name);
        }

        [TestMethod]
        public void TranslateDtoWithConstructorWifhDefaultCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories().Select(c => new CategoryDto(c.Id, c.Name, c.Description))
                .Localize()
                .ToList();

            Assert.AreEqual("Bières", beerCategory[0].Name);
            Assert.AreEqual("Vins", beerCategory[1].Name);
            Assert.AreEqual("Aliments", beerCategory[2].Name);
        }

        [TestMethod]
        public void TranslateGetsFallbackTranslation()
        {
            var foodCateogory = GetCategories().Where(c => c.Id == 3)
                .Select(c => new {ID = c.Id, c.Name})
                .Localize(new CultureInfo("ro-RO"))
                .First();

            Assert.AreEqual("Foods", foodCateogory.Name);
        }

        [TestMethod]
        public void TranslateGetsFallbackTranslationWithDirectPropertyProjection()
        {
            var foodCateogory = GetCategories().Where(c => c.Id == 3)
                .Select(c => c.Name)
                .Localize(new CultureInfo("ro-RO"))
                .First();

            Assert.AreEqual("Foods", foodCateogory);
        }

        [TestMethod]
        public void TranslateWifhExplicitCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories().Where(c => c.Id == 1)
                .Select(c => new {ID = c.Id, c.Name, c.Description})
                .Localize(new CultureInfo("ro-RO"))
                .First();

            Assert.AreEqual("Beri", beerCategory.Name);
        }

        [TestMethod]
        public void TranslateWithDefaultCultureGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories().Where(c => c.Id == 1)
                .Select(c => new {ID = c.Id, c.Name})
                .Localize()
                .First();

            Assert.AreEqual("Bières", beerCategory.Name);
        }

        [TestMethod]
        public void TranslateWorksWithDirectPropertyProjection()
        {
            var foodCateogory = GetCategories().Select(c => c.Name)
                .Localize()
                .ToList();

            Assert.AreEqual("Bières", foodCateogory[0]);
            Assert.AreEqual("Vins", foodCateogory[1]);
            Assert.AreEqual("Aliments", foodCateogory[2]);
        }

        protected static CultureInfo DefaultTestCulture()
        {
            return new CultureInfo("fr-FR");
        }

        protected abstract IQueryable<Category> GetCategories();
    }


    public class CategoryDto
    {
        public CategoryDto()
        {
        }

        public CategoryDto(int id, string name, string description)
        {
            Id   = id;
            Name = name;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}