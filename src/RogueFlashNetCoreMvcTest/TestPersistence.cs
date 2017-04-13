using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.Model.Planner;
using System;

namespace RogueFlashNetCoreMvcTest
{
    public class TestPersistence
    {
        private AppDbContext dbContext = null;


        [SetUp]
        public void SetUp()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("settings.json");
            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            dbContext = new AppDbContext(
                optionsBuilder.Options,
                null);

            var serviceProvider = dbContext.GetInfrastructure<IServiceProvider>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddDebug();
        }

        [Test]
        public void Test()
        {
            var deck = InitializaDeck();

            dbContext.Decks.Add(deck);
            dbContext.SaveChanges();

            var loadedDeck = dbContext.Decks.Find(deck.Id);

            Assert.IsNotNull(loadedDeck);
            
            dbContext.Decks.Remove(loadedDeck);
            dbContext.SaveChanges();

            loadedDeck = dbContext.Decks.Find(deck.Id);

            Assert.IsNull(loadedDeck);
        }


        private Deck InitializaDeck()
        {
            Deck deck = new Deck("Test Deck");
            
            Card card = deck.AddCard();
            card.SideA = "sideA - 你好";
            card.SideB = "sideB - 你好";
            card.Notes = "notes - 你好";
            card.Tags = "tags";

            CardInstance sideAToB = card.GetInstanceSideAToB();
            CardPlan sideAToBPlan = sideAToB.Plan;
            sideAToBPlan.LastDays = 1;
            CardReview sideAToBValue = sideAToB.AddReview();
            sideAToBValue.DateTime = DateTimeOffset.Now;
            sideAToBValue.Value = ReviewValues.Values.VALUE_1;

            CardInstance sideBToA = card.GetInstanceSideBToA();
            CardPlan sideBToAPlan = sideBToA.Plan;
            sideBToAPlan.LastDays = 1;
            CardReview sideBToAValue = sideBToA.AddReview();
            sideBToAValue.DateTime = DateTimeOffset.Now;
            sideBToAValue.Value = ReviewValues.Values.VALUE_1;
            
            return deck;
        }
    }
}
