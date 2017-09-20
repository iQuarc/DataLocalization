using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iQuarc.DataLocalization.Tests.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public List<CateogoryLocalization> Localizations { get; set; }
    }
}