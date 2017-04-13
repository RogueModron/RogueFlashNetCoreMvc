using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using System;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AjaxGetCardToReviewController : AbstractController
    {
        public AjaxGetCardToReviewController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpGet]
        public async Task<JsonResult> Index(int deckId)
        {
            try
            {
                return await buildJsonResult(deckId);
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);

                return GetEmptyJsonError();
            }
        }


        private async Task<JsonResult> buildJsonResult(int deckId)
        {
            if (deckId == 0)
            {
                return GetEmptyJson();
            }

            CardInstance cardInstance = null;
            using (var cardDao = new CardDao(DbContext))
            {
                cardInstance = await cardDao.GetNextCardInstanceToReview(deckId);
            }

            if (cardInstance == null)
            {
                return GetEmptyJson();
            }

            var card = cardInstance.Card;

            var result = new
            {
                cardId = card.Id,
                sideA = card.SideA,
                sideB = card.SideB,
                notes = card.Notes,
                tags = card.Tags,
                cardInstanceId = cardInstance.Id,
                sideAToB = cardInstance.SideAToB
            };

            return Json(result);
        }
    }
}
