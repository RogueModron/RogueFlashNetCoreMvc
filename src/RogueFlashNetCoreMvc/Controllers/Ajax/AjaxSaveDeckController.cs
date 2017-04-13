using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using System;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AjaxSaveDeckController : AbstractController
    {
        public AjaxSaveDeckController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpPost]
        public async Task<JsonResult> Index(int deckId)
        {
            try
            {
                await SaveDeck(deckId);

                return GetEmptyJson();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);

                return GetEmptyJsonError();
            }
        }


        private async Task SaveDeck(int deckId)
        {
            using (var deckDao = new DeckDao(DbContext))
            {
                var deck = await deckDao.GetDeck(deckId);
                if (deck == null)
                {
                    return;
                }

                var updated = await TryUpdateModelAsync(
                    deck,
                    "",
                    e => e.Description,
                    e => e.Notes);
                if (!updated)
                {
                    throw new ArgumentException();
                }

                await deckDao.UpdateDeck(deck);
            }
        }
    }
}
