using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.ViewData;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class CardController : AbstractController
    {
        public CardController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpGet]
        public async Task<IActionResult> Index(
                int deckId,
                int cardId)
        {
            CardData cardData = null;

            if (cardId != 0)
            {
                cardData = await GetCard(cardId);
            }
            else
            {
                if (deckId != 0)
                {
                    cardData = await GetNewCard(deckId);
                }
            }

            if (cardData != null)
            {
                ViewData["Title"] = "Card";

                return View(cardData);
            }
            else
            {
                return RedirectToAction("", "Decks");
            }
        }


        private async Task<CardData> GetCard(int cardId)
        {
            Card card = null;
            using (var cardDao = new CardDao(DbContext))
            {
                card = await cardDao.GetCardWithInstances(cardId);
            }

            return GetCardData(card);
        }

        private async Task<CardData> GetNewCard(int deckId)
        {
            Card card = null;
            using (var deckDao = new DeckDao(DbContext))
            {
                var deck = await deckDao.GetDeck(deckId);
                card = deck.AddCard();
                await deckDao.UpdateDeck(deck);
            }

            return GetCardData(card);
        }

        private CardData GetCardData(Card card)
        {
            CardData cardData = null;
            if (card != null)
            {
                cardData = new CardData();
                cardData.DeckId = card.DeckId;
                cardData.CardId = card.Id;
                cardData.SideA = card.SideA;
                cardData.SideB = card.SideB;
                cardData.Notes = card.Notes;
                cardData.Tags = card.Tags;
                cardData.SideBToA = !card.GetInstanceSideBToA().Disabled;
            }
            return cardData;
        }
    }
}
