using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.ViewData;
using System.Threading.Tasks;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class ReviewController : AbstractController
    {
        public ReviewController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpGet]
        public async Task<IActionResult> Index(int deckId)
        {
            long numberOfCardInstancesToReview = await GetNumberOfCardInstancesToReview(deckId);

            if (numberOfCardInstancesToReview > 0)
            {
                ViewData["Title"] = "Review";

                var reviewData = new ReviewData();
                reviewData.DeckId = deckId;

                return View(reviewData);
            }
            else
            {
                return RedirectToAction("", "Deck",
                    new { deckId = deckId });
            }
        }

        private async Task<long> GetNumberOfCardInstancesToReview(int deckId)
        {
            long numberOfCardInstancesToReview = 0;

            if (deckId != 0)
            {
                using (var deckDao = new DeckDao(DbContext))
                {
                    numberOfCardInstancesToReview = await deckDao.GetNumberOfCardInstancesToReview(deckId);
                }
            }

            return numberOfCardInstancesToReview;
        }
    }
}
