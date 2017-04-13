using System;

namespace RogueFlashNetCoreMvc.Model.Planner
{
    public class PlannerResult
    {
        public DateTimeOffset? NextDate { get; set; } = null;
        public int DaysNext             { get; set; } = 0;

        public int PassedDays           { get; set; } = 0;


        public PlannerResult()
        {
            //
        }

        public PlannerResult(
                DateTimeOffset nextDate,
                int daysNext,
                int passedDays)
        {
            this.NextDate = nextDate;
            this.DaysNext = daysNext;
            this.PassedDays = passedDays;
        }
    }
}
