using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.Model.Unmapped;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AjaxFilterCardsController : AbstractController
    {
        public AjaxFilterCardsController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpGet]
        public async Task<JsonResult> Index(
                int deckId,
                string filterText,
                int firstResult,
                int maxResults)
        {
            try
            {
                return await buildJsonResult(
                    deckId,
                    filterText,
                    firstResult,
                    maxResults);
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);

                return GetEmptyJsonError();
            }
        }


        private async Task<JsonResult> buildJsonResult(
                int deckId,
                string filterText,
                int firstResult,
                int maxResults)
        {
            if (deckId == 0)
            {
                return new JsonResult(new object());
            }

            if (filterText == null)
            {
                filterText = "";
            }
            if (maxResults > 100)
            {
                maxResults = 100;
            }

            List<FindCardsResult> cards = null;
            using (var cardDao = new CardDao(DbContext))
            {
                cards = await cardDao.FindCards(
                    deckId,
                    filterText,
                    firstResult,
                    maxResults);
            }

            var resultList = new List<object>();
            foreach (var card in cards)
            {
                resultList.Add(new
                {
                    cardId = card.CardId,
                    sideA = card.SideA,
                    sideB = card.SideB,
                    notes = card.Notes,
                    tags = card.Tags
                });
            }

            return Json(resultList);
        }
    }
}
