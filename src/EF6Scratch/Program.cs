using EF6Scratch.Model;
using System.Linq;

namespace EF6Scratch
{
    class Program
    {
        private static AppDbContext getDbContext()
        {
            var dbContext = new AppDbContext("Default");
            //dbContext.Database.Log = Console.Write;
            dbContext.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            return dbContext;
        }

        static void Main(string[] args)
        {
            var dbContext = getDbContext();

            var result = dbContext.Decks
                .GroupJoin(dbContext.Cards,
                    l => l.Id,
                    r => r.DeckId,
                    (l, r) => new {
                        Deck = l,
                        Card = r.DefaultIfEmpty()
                    }
                )
                .SelectMany(
                    j => j.Card.Select(card => new {
                        Deck = j.Deck,
                        Card = card
                    })
                )
                .GroupJoin(dbContext.CardsInstances,
                    l => l.Card.Id,
                    r => r.CardId,
                    (l, r) => new {
                        DeckCard = l,
                        CardInstance = r.DefaultIfEmpty()
                    }
                )
                .SelectMany(
                    j => j.CardInstance.Select(instance => new {
                        Instance = instance.Id
                    })
                )
                .ToList();

            System.Console.WriteLine("Ok");
        }
    }
}
