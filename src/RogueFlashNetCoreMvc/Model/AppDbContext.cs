using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RogueFlashNetCoreMvc.Model
{
    public class AppDbContext : DbContext
    {
        private ILoggerFactory loggerFactory = null;


        // In EF 6:
        // https://msdn.microsoft.com/en-us/data/jj592675
        public DbSet<Deck> Decks                    => Set<Deck>();
        public DbSet<Card> Cards                    => Set<Card>();
        public DbSet<CardInstance> CardsInstances   => Set<CardInstance>();
        public DbSet<CardPlan> CardsPlans           => Set<CardPlan>();
        public DbSet<CardReview> CardsReviews       => Set<CardReview>();


        public AppDbContext(
                DbContextOptions<AppDbContext> options,
                ILoggerFactory loggerFactory)
            : base(options)
        {
            this.loggerFactory = loggerFactory;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // In EF Core 1.1:
            // https://blog.oneunicorn.com/2016/10/28/collection-navigation-properties-and-fields-in-ef-core-1-1/
            //
            // So Card could have a private _Instances property backing Instances, and it must be set OnModelCreating:
            //
            //      modelBuilder.
            //          Entity<Card>()
            //          .Metadata
            //          .FindNavigation(nameof(Card.Instances))
            //          .SetPropertyAccessMode(PropertyAccessMode.Field);
            //
            // Then the navigation property is correctly used when saving but not when querying:
            //
            //      var t = dbContext.Cards
            //          .Include(j => j.Instances)
            //          .First(j => j.Id == 16);
            //
            // Infact, in this case Instances is always empty.
            //
            // Hence, for uniformity, and in order to avoid using reflection, all the model's classes don't have backing fields.
            //

            var deckBuilder = modelBuilder.Entity<Deck>();
            deckBuilder.Property(e => e.Version).IsConcurrencyToken();

            var cardBuilder = modelBuilder.Entity<Card>();
            cardBuilder.Property(e => e.Version).IsConcurrencyToken();

            // The db is shared with other projects and has camel case identifiers.
            TransformColumnNamesFirstCharToLower(modelBuilder);
        }


        private void TransformColumnNamesFirstCharToLower(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var columnName = char.ToLower(property.Name[0]) + property.Name.Substring(1);
                    property.Relational().ColumnName = columnName;
                }
            }
        }
    }
}
