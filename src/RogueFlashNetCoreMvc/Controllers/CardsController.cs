using Microsoft.AspNetCore.Mvc;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.ViewData;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class CardsController : AbstractController
    {
        public CardsController(AppDbContext dbContext)
            : base(dbContext)
        {
            //
        }


        [HttpGet]
        public IActionResult Index(int deckId)
        {
            if (deckId != 0)
            {
                ViewData["Title"] = "Cards";

                var cardsData = new CardsData();
                cardsData.DeckId = deckId;

                return View(cardsData);
            }
            else
            {
                return RedirectToAction("", "Decks");
            }
        }
    }
}
