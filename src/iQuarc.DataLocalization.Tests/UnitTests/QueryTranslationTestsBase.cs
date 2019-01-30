using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using iQuarc.DataLocalization.Tests.Model;
using Microsoft.EntityFrameworkCore;
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

            var categories = GetCategories()
                .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
                .Localize()
                .OrderBy(x => x.Name)
                .ToList();

            Assert.AreEqual("Aliments", categories[0].Name);
            Assert.AreEqual("Bières", categories[1].Name);
            Assert.AreEqual("Vins", categories[2].Name);
        }

        [TestMethod]
        public void TranslateGetsFallbackTranslation()
        {
            var foodCateogory = GetCategories()
                .Where(c => c.Id == 3)
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

        // -------
        [TestMethod]
        public async Task TranslateDtoWithConstructorWifhDefaultCultureGetsTheCorrectTranslationAsync()
        {

            var categories = await GetCategories()
                .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
                .Localize()
                .OrderBy(x => x.Name)
                .ToListAsync();

            Assert.AreEqual("Aliments", categories[0].Name);
            Assert.AreEqual("Bières", categories[1].Name);
            Assert.AreEqual("Vins", categories[2].Name);
        }

        [TestMethod]
        public async Task TranslateGetsFallbackTranslationAsync()
        {
            var foodCateogory = await GetCategories()
                .Where(c => c.Id == 3)
                .Select(c => new { ID = c.Id, c.Name })
                .Localize(new CultureInfo("ro-RO"))
                .FirstAsync();

            Assert.AreEqual("Foods", foodCateogory.Name);
        }

        [TestMethod]
        public async Task TranslateGetsFallbackTranslationWithDirectPropertyProjectionAsync()
        {
            var foodCateogory = await GetCategories().Where(c => c.Id == 3)
                .Select(c => c.Name)
                .Localize(new CultureInfo("ro-RO"))
                .FirstAsync();

            Assert.AreEqual("Foods", foodCateogory);
        }

        [TestMethod]
        public async Task TranslateWifhExplicitCultureGetsTheCorrectTranslationAsync()
        {
            var beerCategory = await GetCategories().Where(c => c.Id == 1)
                .Select(c => new { ID = c.Id, c.Name, c.Description })
                .Localize(new CultureInfo("ro-RO"))
                .FirstAsync();

            Assert.AreEqual("Beri", beerCategory.Name);
        }

        [TestMethod]
        public async Task TranslateWithDefaultCultureGetsTheCorrectTranslationAsync()
        {
            var beerCategory = await GetCategories().Where(c => c.Id == 1)
                .Select(c => new { ID = c.Id, c.Name })
                .Localize()
                .FirstAsync();

            Assert.AreEqual("Bières", beerCategory.Name);
        }

        [TestMethod]
        public async Task TranslateWorksWithDirectPropertyProjectionAsync()
        {
            var foodCateogory = await GetCategories().Select(c => c.Name)
                .Localize()
                .ToListAsync();

            Assert.AreEqual("Bières", foodCateogory[0]);
            Assert.AreEqual("Vins", foodCateogory[1]);
            Assert.AreEqual("Aliments", foodCateogory[2]);
        }
        // -------

        protected static CultureInfo DefaultTestCulture()
        {
            return new CultureInfo("fr-FR");
        }

        protected abstract IQueryable<Category> GetCategories();

        protected abstract IQueryable<CategoryLocalization> GetCategoryLocalizations();
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