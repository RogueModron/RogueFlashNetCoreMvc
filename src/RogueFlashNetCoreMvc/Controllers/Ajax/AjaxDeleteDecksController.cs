using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using System;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AjaxDeleteDecksController : AbstractController
    {
        public AjaxDeleteDecksController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpDelete]
        public async Task<JsonResult> Index()
        {
            try
            {
                await DeleteDecks();

                return GetEmptyJson();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);

                return GetEmptyJsonError();
            }
        }


        private async Task DeleteDecks()
        {
            var ids = GetIntArrayParameter("ids");
            if (ids.Length == 0)
            {
                return;
            }

            using (var deckDao = new DeckDao(DbContext))
            {
                await deckDao.DeleteDecks(ids);
            }
        }
    }
}
