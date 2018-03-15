using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iQuarc.DataLocalization.Tests.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<ProductLocalization> Localizations { get; set; } = new List<ProductLocalization>();
    }
}