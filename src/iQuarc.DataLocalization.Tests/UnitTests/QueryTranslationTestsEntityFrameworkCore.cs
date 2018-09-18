using System.Linq;
using iQuarc.DataLocalization.Tests.DataBase;
using iQuarc.DataLocalization.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iQuarc.DataLocalization.Tests.UnitTests
{
    [TestClass]
    public class QueryTranslationTestsEntityFrameworkCore : QueryTranslationTestsBase
    {
        private TranslationContext ctx;

        [ClassInitialize]
        public static void Setup(TestContext ctx)
        {
            LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.IsoCode);
            LocalizationConfig.RegisterLocalizationProvider(DefaultTestCulture);
            LocalizationConfig.RegisterCultureMapper(c => c.TwoLetterISOLanguageName);
        }

        [TestInitialize]
        public void Initialize()
        {
            this.ctx = new TranslationContext();
        }

        protected override IQueryable<Category> GetCategories()
        {
            return ctx.Categories;
        }
    }
}