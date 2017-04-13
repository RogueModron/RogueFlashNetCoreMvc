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
    public class AjaxFilterDecksController : AbstractController
    {
        public AjaxFilterDecksController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpGet]
        public async Task<JsonResult> Index(
                string filterText,
                int firstResult,
                int maxResults)
        {
            try
            {
                return await buildJsonResult(
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
                string filterText,
                int firstResult,
                int maxResults)
        {
            if (filterText == null)
            {
                filterText = "";
            }
            if (maxResults > 100)
            {
                maxResults = 100;
            }

            List<FindDecksResult> decks = null;
            using (var deckDao = new DeckDao(DbContext))
            {
                decks = await deckDao.FindDecks(
                   filterText,
                   firstResult,
                   maxResults);
            }

            var resultList = new List<object>();
            foreach (var deck in decks)
            {
                resultList.Add(new
                {
                    deckId = deck.DeckId,
                    description = deck.Description,
                    notes = deck.Notes,
                    numberOfSides = deck.NumberOfSides
                });
            }

            return Json(resultList);
        }
    }
}
