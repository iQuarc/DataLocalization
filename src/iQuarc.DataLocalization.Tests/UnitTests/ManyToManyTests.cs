using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using iQuarc.DataLocalization.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iQuarc.DataLocalization.Tests.UnitTests
{
    [TestClass]
    public class ManyToManyTests
    {
        private List<Category> categories;
        private List<Product> products;

        [ClassInitialize]
        public static void Setup(TestContext ctx)
        {
            LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.IsoCode);
            LocalizationConfig.RegisterLocalizationProvider(DefaultTestCulture);
            LocalizationConfig.RegisterCultureMapper(c => c.TwoLetterISOLanguageName);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            CreateSample();
        }

        [TestMethod]
        public void ManyToManyCategoryWithListProjectionGetsTheCorrectTranslation()
        {
            var beerCategory = GetCategories()
                .Where(c => c.Id == 1)
                .Select(c => new
                {
                    ID = c.Id,
                    Name = c.Name,
                    Producs = c.Products.Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                    }).ToList()
                })
                .Localize(DefaultTestCulture())
                .First();

            Assert.AreEqual("Bières", beerCategory.Name);
            Assert.AreEqual("Combinaison bière et frites", beerCategory.Producs[0].Name);
        }


        [TestMethod]
        public void ManyToManyProductsWithListProjectionGetsTheCorrectTranslation()
        {
            var products = GetProducts()
                .Select(c => new
                {
                    ID = c.Id,
                    Name = c.Name,
                    Categories = c.Categories.Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                    }).ToList()
                })
                .Localize(new CultureInfo("ro-RO"))
                .ToList();

            Assert.AreEqual("Bere și chips combo", products[0].Name);
            Assert.AreEqual("Beri", products[0].Categories[0].Name);
            Assert.AreEqual("Foods", products[0].Categories[1].Name);

            Assert.AreEqual("Selecție de vinuri și brânză", products[1].Name);
            Assert.AreEqual("Vinuri", products[1].Categories[0].Name);
            Assert.AreEqual("Foods", products[1].Categories[1].Name);
        }


        [TestMethod]
        public void ManyToManyProductsWithCategoriesListProjectionGetsTheCorrectTranslation()
        {
            var products = GetProducts()
                .Where(p => p.Id == 1)
                .Select(p => new
                {
                    ID = p.Id,
                    Name = p.Name,
                    Categories = p.Categories.Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Products = c.Products
                                .Where(p2 => p2.Id == 1)
                                .Select(c2 => new
                                        {
                                            Name = c2.Name,
                                        })
                                        .ToList()
                    }).ToList()
                })
                .Localize(new CultureInfo("fr-FR"))
                .ToList();

            Assert.AreEqual("Combinaison bière et frites", products[0].Name);
            Assert.AreEqual("Bières", products[0].Categories[0].Name);
            Assert.AreEqual("Aliments", products[0].Categories[1].Name);
            Assert.AreEqual("Combinaison bière et frites", products[0].Categories[1].Products[0].Name);
            
        }

        static CultureInfo DefaultTestCulture()
        {
            return new CultureInfo("fr-FR");
        }

        IQueryable<Category> GetCategories()
        {
            return categories.AsQueryable();
        }

        IQueryable<Product> GetProducts()
        {
            return products.AsQueryable();
        }

        private void CreateSample()
        {
            var fr = new Language { Id = 1, IsoCode = "fr", Name = "French" };
            var ro = new Language { Id = 2, IsoCode = "ro", Name = "Romanian" };

            var cat1 = new Category
            {
                Id = 1,
                Name = "Beers",
                Localizations = new List<CategoryLocalization>
                    {
                        new CategoryLocalization {CategoryId = 1, LanguageId = 1, Language = fr, Name = "Bières"},
                        new CategoryLocalization {CategoryId = 1, LanguageId = 2, Language = ro, Name = "Beri"}
                    }
            };
            var cat2 = new Category
            {
                Id = 2,
                Name = "Wines",
                Localizations = new List<CategoryLocalization>
                    {
                        new CategoryLocalization {CategoryId = 2, LanguageId = 1, Language = fr, Name = "Vins"},
                        new CategoryLocalization {CategoryId = 2, LanguageId = 2, Language = ro, Name = "Vinuri"}
                    }
            };
            var cat3 = new Category
            {
                Id = 3,
                Name = "Foods",
                Localizations = new List<CategoryLocalization>
                    {
                        new CategoryLocalization {CategoryId = 3, LanguageId = 1, Language = fr, Name = "Aliments"},
                    }
            };

            var prod1 = new Product
            {
                Id = 1,
                Name = "Beer & Chips combo",
                Categories = new List<Category> { cat1, cat3 },
                Localizations = new List<ProductLocalization>
                    {
                        new ProductLocalization {ProductId = 1, LanguageId = 1, Language = fr, Name = "Combinaison bière et frites"},
                        new ProductLocalization {ProductId = 1, LanguageId = 2, Language = ro, Name = "Bere și chips combo"},
                    }
            };

            var prod2 = new Product
            {
                Id = 2,
                Name = "Wine and cheese selection",
                Categories = new List<Category> { cat2, cat3 },
                Localizations = new List<ProductLocalization>
                    {
                        new ProductLocalization {ProductId = 2, LanguageId = 1, Language = fr, Name = "Sélection de vins et fromages"},
                        new ProductLocalization {ProductId = 2, LanguageId = 2, Language = ro, Name = "Selecție de vinuri și brânză"},
                    }
            };

            cat1.Products.Add(prod1);
            cat2.Products.Add(prod2);
            cat3.Products.Add(prod1);
            cat3.Products.Add(prod2);

            categories = new List<Category>();
            categories.Add(cat1);
            categories.Add(cat2);
            categories.Add(cat3);

            products = new List<Product>();
            products.Add(prod1);
            products.Add(prod2);
        }
    }

    public class ProductDto
    {
        public ProductDto()
        {


        }
        public ProductDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}