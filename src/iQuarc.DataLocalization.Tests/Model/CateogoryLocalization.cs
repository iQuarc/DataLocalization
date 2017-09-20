using System.ComponentModel.DataAnnotations;

namespace iQuarc.DataLocalization.Tests.Model
{
    [TranslationFor(typeof(Category))]
    public class CateogoryLocalization
    {
        [Key]
        public int CategoryId { get; set; }
        [Key]
        public int LanguageId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public Category Category { get; set; }
        public Language Language { get; set; }
    }
}