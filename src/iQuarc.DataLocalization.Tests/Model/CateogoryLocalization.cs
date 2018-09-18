using System.ComponentModel.DataAnnotations;

namespace iQuarc.DataLocalization.Tests.Model
{
    [TranslationFor(typeof(Category))]
    public class CategoryLocalization
    {
        [Key]
        public int CategoryId { get; set; }
        [Key]
        public int LanguageId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string CommercialName { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [MaxLength(512)]
        public string ShortDescription { get; set; }

        public Category Category { get; set; }
        public Language Language { get; set; }
    }
}