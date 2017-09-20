using System.ComponentModel.DataAnnotations;

namespace iQuarc.DataLocalization.Tests.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public int Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}