using Microsoft.EntityFrameworkCore;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.Model.Unmapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Daos
{
    public class DeckDao : AbstractDao
    {
        public DeckDao(AppDbContext dbContext)
            : base(dbContext)
        {
            //
        }


        public async Task<bool> CheckDecksExistance()
        {
            var id = await DbContext.Decks
                .Select(e => e.Id)
                .FirstOrDefaultAsync();
            return id > 0;
        }

        public async Task DeleteDeck(int deckId)
        {
            var deck = await DbContext.Decks
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == deckId);
            DbContext.Decks.Remove(deck);
            await DbContext.SaveChangesAsync();
	    }

        public async Task DeleteDecks(int[] decksIds)
        {
            foreach (var id in decksIds)
            {
                using (var transaction = await DbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await DeleteDeck(id);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<List<FindDecksResult>> FindDecks(
                string descriptionFilter,
                int firstResult,
                int maxResults)
        {
            if (descriptionFilter == null)
            {
                throw new ArgumentException();
            }
            if (firstResult < 0)
            {
                throw new ArgumentException();
            }
            if (maxResults <= 0)
            {
                throw new ArgumentException();
            }

            var q = DbContext.Decks.AsQueryable();

            if (!string.IsNullOrEmpty(descriptionFilter))
            {
                q = q.Where(e => e.Description.Contains(descriptionFilter)).AsQueryable();
            }

            // Does not work with Microsoft.EntityFrameworkCore version 1.1.1
            // but it's ok with version 1.1.0:
            //  https://github.com/aspnet/EntityFramework/issues/7714

            return await q
                .Select(e => new FindDecksResult
                    {
                        DeckId = e.Id,
                        Description = e.Description,
                        Notes = e.Notes,
                        NumberOfSides = DbContext.Cards
                            .Join(DbContext.CardsInstances,
                                l => l.Id,
                                r => r.CardId,
                                (l, r) => new
                                {
                                    DeckId = l.DeckId,
                                    InstanceDisabled = r.Disabled
                                }
                            )
                            .Where(j => j.DeckId == e.Id)
                            .Where(j => !j.InstanceDisabled)
                            .Count()
                    }
                )
                .OrderBy(s => s.DeckId)
                .OrderBy(s => s.Description)
                .Skip(firstResult)
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<Deck> GetDeck(int deckId)
        {
            return await DbContext.Decks.FindAsync(deckId);
        }

        public async Task<long> GetNumberOfCardInstancesToReview(int deckId)
        {
            var now = DateTimeOffset.Now;
            return await DbContext.Cards
                .Join(DbContext.CardsInstances,
                    l => l.Id,
                    r => r.CardId,
                    (l, r) => new
                    {
                        Card = l,
                        Instance = r
                    }
                )
                .Join(DbContext.CardsPlans,
                    l => l.Instance.Id,
                    r => r.CardInstanceId,
                    (l, r) => new
                    {
                        Card = l.Card,
                        Instance = l.Instance,
                        CardPlan = r
                    }
                )
                .Where(j => j.Card.DeckId == deckId)
                .Where(j => !j.Instance.Disabled)
                .Where(j => !string.IsNullOrEmpty(j.Card.SideA) &&
                    !string.IsNullOrEmpty(j.Card.SideB))
                .Where(j => !j.CardPlan.NextDate.HasValue ||
                    (j.CardPlan.NextDate.HasValue && j.CardPlan.NextDate <= now))
                .CountAsync();
        }

        public async Task InsertDeck(Deck deck)
        {
            if (deck.Id != 0)
            {
                throw new ArgumentException();
            }

            DbContext.Decks.Add(deck);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateDeck(Deck deck)
        {
            if (deck.Id == 0)
            {
                throw new ArgumentException();
            }

            deck.Version++;

            await DbContext.SaveChangesAsync();
        }
    }
}
