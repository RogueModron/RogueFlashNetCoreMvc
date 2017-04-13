using System;

namespace RogueFlashNetCoreMvc.Model.Planner
{
    public class Planner
    {
        public const double PASSED_DAYS_MAX = 10000;
        public const double PASSED_DAYS_MIN = 0;


        public static PlannerResult PlanNext(
                ReviewValues.Values value,
                DateTimeOffset valueDate,
                DateTimeOffset? previousDate)
        {
            double passedDays = 0;
            if (previousDate != null)
            {
                passedDays = Math.Truncate(valueDate.Subtract(previousDate.Value).TotalDays);
            }
            if (!CheckPassedDaysLimits(passedDays))
            {
                throw new ArgumentException();
            }
            
            int nextDays = CalculateNextDays(value, (int) passedDays);
            DateTimeOffset nextDate = valueDate.AddDays(nextDays);

            PlannerResult plannerResult = new PlannerResult(
                nextDate,
                nextDays,
                (int) passedDays);
            return plannerResult;
        }


        private static int CalculateNextDays(
                ReviewValues.Values value,
                int passedDays)
        {
            if (value == ReviewValues.Values.VALUE_0)
            {
                return 0;
            }

            ReviewValues.ReviewValue reviewValue = ReviewValues.GetReviewValueFromValue(value);
            double daysNext = Math.Floor(passedDays * reviewValue.DaysMultiplier + reviewValue.DaysBase);
            return (int) Math.Round(daysNext, MidpointRounding.AwayFromZero);
        }

        private static bool CheckPassedDaysLimits(double passedDays)
        {
            return PASSED_DAYS_MIN <= passedDays && passedDays <= PASSED_DAYS_MAX;
        }
    }
}
