using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Daos;
using RogueFlashNetCoreMvc.Model;
using RogueFlashNetCoreMvc.Model.Planner;
using System;
using System.Threading.Tasks;
using static RogueFlashNetCoreMvc.Model.Planner.ReviewValues;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AjaxSaveReviewController : AbstractController
    {
        public AjaxSaveReviewController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
            : base(dbContext, loggerFactory)
        {
            //
        }


        [HttpPost]
        public async Task<JsonResult> Index(int cardInstanceId)
        {
            try
            {
                await SaveReview(cardInstanceId);

                return GetEmptyJson();
            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message);

                return GetEmptyJsonError();
            }
        }


        private async Task SaveReview(int cardInstanceId)
        {
            using (var cardDao = new CardDao(DbContext))
            {
                CardInstance cardInstance = await cardDao.GetCardInstanceWithPlan(cardInstanceId);
                if (cardInstance == null)
                {
                    return;
                }

                DateTimeOffset now = DateTimeOffset.Now;
                CardPlan plan = cardInstance.Plan;
                if (plan.NextDate != null
                        && now < plan.NextDate)
                {
                    return;
                }

                var value = int.Parse(GetParameter("value"));
                Values reviewValue = GetValueFromOrdinal(value);

                PlannerResult plannerResult = Planner.PlanNext(
                    reviewValue,
                    now,
                    plan.LastDate);

                plan.LastDate = now;
                plan.LastDays = plannerResult.PassedDays;
                plan.NextDate = plannerResult.NextDate;
                plan.NextDays = plannerResult.DaysNext;

                var review = cardInstance.AddReview();
                review.DateTime = now;
                review.Value = reviewValue;

                await cardDao.UpdateCardInstance(cardInstance);
            }
        }
    }
}
