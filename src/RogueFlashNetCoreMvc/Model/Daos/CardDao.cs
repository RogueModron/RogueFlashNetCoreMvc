using Microsoft.EntityFrameworkCore;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.Model.Unmapped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Daos
{
    public class CardDao : AbstractDao
    {
        public CardDao(AppDbContext dbContext)
            : base(dbContext)
        {
            //
        }


        public async Task<bool> CheckCardsExistance(int deckId)
        {
            var id = await DbContext.Cards
                .GroupJoin(DbContext.Decks,
                    l => l.DeckId,
                    r => r.Id,
                    (l, r) => new
                    {
                        l.DeckId
                    }
                )
                .Where(j => j.DeckId == deckId)
                .Select(j => j.DeckId)
                .FirstOrDefaultAsync();
            return id > 0;
        }

        public async Task DeleteCard(int cardId)
        {
            var card = await DbContext.Cards
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == cardId);
            DbContext.Cards.Remove(card);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteCards(int[] cardsIds)
        {
            foreach (var id in cardsIds)
            {
                using (var transaction = await DbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await DeleteCard(id);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<List<FindCardsResult>> FindCards(
                int deckId,
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

            var q = DbContext.Cards
                .Join(DbContext.CardsInstances,
                    l => l.Id,
                    r => r.CardId,
                    (l, r) => new
                    {
                        Card = l,
                        Instance = r
                    }
                )
                .Where(j => j.Instance.SideAToB)
                .AsQueryable();

            if (deckId > 0)
            {
                q = q.Where(j => j.Card.DeckId == deckId).AsQueryable();
            }

            if (!string.IsNullOrEmpty(descriptionFilter))
            {
                q = q
                    .Where(j => j.Card.SideA.Contains(descriptionFilter) ||
                        j.Card.SideB.Contains(descriptionFilter) ||
                        j.Card.Tags.Contains(descriptionFilter)
                    ).AsQueryable();
            }

            return await q
                .Select(j => new FindCardsResult
                    {
                        CardId = j.Card.Id,
                        SideA = j.Card.SideA,
                        SideB = j.Card.SideB,
                        Notes = j.Card.Notes,
                        Tags = j.Card.Tags,
                        SideAToBDisabled = j.Instance.Disabled
                    }
                )
                .OrderBy(s => s.Tags)
                .OrderBy(s => s.SideA)
                .OrderBy(s => s.SideB)
                .OrderBy(s => s.CardId)
                .Skip(firstResult)
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<Card> GetCard(int cardId)
        {
            return await DbContext.Cards.FindAsync(cardId);
        }

        public async Task<CardInstance> GetCardInstance(int cardInstanceId)
        {
            return await DbContext.CardsInstances.FindAsync(cardInstanceId);
        }

        public async Task<Card> GetCardWithInstances(int cardId)
        {
            return await DbContext.Cards
                .Include(e => e.Instances)
                .FirstAsync(e => e.Id == cardId);
        }

        public async Task<CardInstance> GetCardInstanceWithPlan(int cardInstanceId)
        {
            return await DbContext.CardsInstances
                .Include(e => e.Plan)
                .FirstAsync(e => e.Id == cardInstanceId);
        }

        public async Task<CardInstance> GetNextCardInstanceToReview(int deckId)
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
                .Select(j => j.Instance)
                .OrderBy(e => e.Plan.NextDate)
                .Include(e => e.Card)
                .FirstAsync();
        }

        public async Task UpdateCard(Card card)
        {
            if (card.Id == 0)
            {
                throw new ArgumentException();
            }

            card.Version++;

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateCardInstance(CardInstance cardInstance)
        {
            if (cardInstance.Id == 0)
            {
                throw new ArgumentException();
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
