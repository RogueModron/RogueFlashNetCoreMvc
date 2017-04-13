using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using System;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AjaxSaveCardController : AbstractController
    {
        public AjaxSaveCardController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpPost]
        public async Task<JsonResult> Index(int cardId)
        {
            try
            {
                await SaveCard(cardId);

                return GetEmptyJson();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);

                return GetEmptyJsonError();
            }
        }


        private async Task SaveCard(int cardId)
        {
            using (var cardDao = new CardDao(DbContext))
            {
                var card = await cardDao.GetCardWithInstances(cardId);
                if (card == null)
                {
                    return;
                }
                
                var updated = await TryUpdateModelAsync(
                    card,
                    "",
                    e => e.SideA,
                    e => e.SideB,
                    e => e.Notes,
                    e => e.Tags);
                if (!updated)
                {
                    throw new ArgumentException();
                }

                card.GetInstanceSideBToA().Disabled = !GetBooleanParameter("sideBToA");
                
                await cardDao.UpdateCard(card);
            }
        }
    }
}
