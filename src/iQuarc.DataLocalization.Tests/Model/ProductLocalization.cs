using System.ComponentModel.DataAnnotations;

namespace iQuarc.DataLocalization.Tests.Model
{
    [TranslationFor(typeof(Product))]
    public class ProductLocalization
    {
        [Key]
        public int ProductId { get; set; }
        [Key]
        public int LanguageId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public Product Product { get; set; }
        public Language Language { get; set; }
    }
}