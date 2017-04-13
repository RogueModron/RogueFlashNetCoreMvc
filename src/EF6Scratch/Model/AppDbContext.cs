using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EF6Scratch.Model
{
    public class AppDbContext : DbContext
    {
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardInstance> CardsInstances { get; set; }
        public DbSet<CardPlan> CardsPlans { get; set; }
        public DbSet<CardReview> CardsReviews { get; set; }


        public AppDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //Database.SetInitializer<AppDbContext>(null);
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add(new FirstCharLowerCaseConvention());

            var deckBuilder = modelBuilder.Entity<Deck>();
            deckBuilder.ToTable("Decks");
            deckBuilder.Property(e => e.Version).IsConcurrencyToken();

            var cardBuilder = modelBuilder.Entity<Card>();
            cardBuilder.ToTable("Cards");
            cardBuilder.Property(e => e.Version).IsConcurrencyToken();

            var cardInstanceBuilder = modelBuilder.Entity<CardInstance>();
            cardInstanceBuilder.ToTable("CardsInstances");

            cardInstanceBuilder
                .HasOptional(e => e.Plan)
                .WithRequired(e => e.Instance);

            var cardPlanBuilder = modelBuilder.Entity<CardPlan>();
            cardPlanBuilder.ToTable("CardsPlans");

            var cardReviewBuilder = modelBuilder.Entity<CardReview>();
            cardReviewBuilder.ToTable("CardsReviews");
        }
    }

    class FirstCharLowerCaseConvention : IStoreModelConvention<EdmProperty>
    {
        public void Apply(EdmProperty property, DbModel model)
        {
            var columnName = char.ToLower(property.Name[0]) + property.Name.Substring(1);
            property.Name = columnName;
        }
    }
}
