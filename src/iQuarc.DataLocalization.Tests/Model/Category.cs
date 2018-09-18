using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iQuarc.DataLocalization.Tests.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [NotMapped]
        public List<Product> Products { get; set; } = new List<Product>();
        public List<CategoryLocalization> Localizations { get; set; } = new List<CategoryLocalization>();
    }
}