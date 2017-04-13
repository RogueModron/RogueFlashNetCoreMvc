using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using System;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AjaxDeleteCardsController : AbstractController
    {
        public AjaxDeleteCardsController(
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
                await DeleteCards();

                return GetEmptyJson();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);

                return GetEmptyJsonError();
            }
        }


        private async Task DeleteCards()
        {
            var ids = GetIntArrayParameter("ids");
            if (ids.Length == 0)
            {
                return;
            }

            using (var cardDao = new CardDao(DbContext))
            {
                await cardDao.DeleteCards(ids);
            }
        }
    }
}
