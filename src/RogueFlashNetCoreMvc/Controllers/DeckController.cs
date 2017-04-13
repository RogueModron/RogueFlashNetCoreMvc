using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.ViewData;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class DeckController : AbstractController
    {
        public DeckController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpGet]
        public async Task<IActionResult> Index(int deckId)
        {
            DeckData deckData = null;

            if (deckId != 0)
            {
                deckData = await GetDeck(deckId);
            }
            else
            {
                deckData = await GetNewDeck();
            }

            if (deckData != null)
            {
                ViewData["Title"] = "Deck";

                return View(deckData);
            }
            else
            {
                return RedirectToAction("", "Decks");
            }
        }


        private async Task<DeckData> GetDeck(int deckId)
        {
            Deck deck = null;
            using (var deckDao = new DeckDao(DbContext))
            {
                deck = await deckDao.GetDeck(deckId);
            }
            
            return GetDeckData(deck);
        }

        private async Task<DeckData> GetNewDeck()
        {
            var deck = new Deck("");
            using (var deckDao = new DeckDao(DbContext))
            {
                await deckDao.InsertDeck(deck);
            }

            return GetDeckData(deck);
        }

        private DeckData GetDeckData(Deck deck)
        {
            DeckData deckData = null;
            if (deck != null)
            {
                deckData = new DeckData();
                deckData.DeckId = deck.Id;
                deckData.Description = deck.Description;
                deckData.Notes = deck.Notes;
            }
            return deckData;
        }
    }
}
