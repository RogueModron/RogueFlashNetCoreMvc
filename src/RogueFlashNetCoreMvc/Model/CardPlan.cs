using System;

namespace RogueFlashNetCoreMvc.Model
{
    public class CardPlan
    {
        public int Id                       { get; protected set; } = 0;

        public int CardInstanceId           { get; set; } = 0;
        public CardInstance Instance        { get; set; } = null;

        public DateTimeOffset? NextDate     { get; set; } = null;
        public int NextDays                 { get; set; } = 0;
        public DateTimeOffset? LastDate     { get; set; } = null;
        public int LastDays                 { get; set; } = 0;


        private CardPlan()
        {
            //
        }

        public CardPlan(CardInstance instance)
        {
            this.Instance = instance;
        }
    }
}
