using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class StartController : AbstractController
    {
        public StartController(AppDbContext dbContext)
            : base(dbContext)
        {
            //
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var deckDao = new DeckDao(DbContext))
            {
                var deckExists = await deckDao.CheckDecksExistance();
                if (deckExists)
                {
                    return RedirectToAction("", "Decks");
                }
                else
                {
                    return RedirectToAction("", "Deck");
                }
            }
        }
    }
}
