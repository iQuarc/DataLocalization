using iQuarc.DataLocalization.Tests.Model;
using Microsoft.EntityFrameworkCore;

namespace iQuarc.DataLocalization.Tests.DataBase
{
    public class TranslationContext : DbContext
    {
        public TranslationContext()
        {
            
        }
        public TranslationContext(DbContextOptions<TranslationContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLocalization> CategoryLocalizations { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DataLocalizationTests;Trusted_Connection=True;ConnectRetryCount=0");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Language>()
                .HasData(
                    new { Id = 1, IsoCode = "fr", ThreeLetterIsoCode = "fra", LCID = 12, Name = "French" },
                    new { Id = 2, IsoCode = "ro", ThreeLetterIsoCode = "ron", LCID = 24, Name = "Romanian" }
            );

            modelBuilder.Entity<Category>()
                .HasData(
                    new { Id = 1, Name = "Beers", Description = "Selection of craft beers" },
                    new { Id = 2, Name = "Wines", Description = "Local and international wines" },
                    new { Id = 3, Name = "Foods", Description = "Bistro foods and snacks" }
            );

            modelBuilder.Entity<CategoryLocalization>()
                .HasKey(x => new {x.CategoryId, x.LanguageId});

            modelBuilder.Entity<CategoryLocalization>()
                .HasData(
                    new { CategoryId = 1, LanguageId = 1, Name = "Bières",   Description= "Sélection de bières artisanales" },
                    new { CategoryId = 1, LanguageId = 2, Name = "Beri",     Description = "Selecţie de bere artizanalã" },
                    new { CategoryId = 2, LanguageId = 1, Name = "Vins",     Description = "Vins locaux et internationaux" },
                    new { CategoryId = 2, LanguageId = 2, Name = "Vinuri",   Description = "Mâncãruri bistro și gustări" },
                    new { CategoryId = 3, LanguageId = 1, Name = "Aliments", Description = "Mets et collations Bistro" }
           );
        }
    }
}