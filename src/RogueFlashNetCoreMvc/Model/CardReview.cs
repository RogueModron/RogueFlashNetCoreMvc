using RogueFlashNetCoreMvc.Model.Planner;
using System;

namespace RogueFlashNetCoreMvc.Model
{
    public class CardReview
    {
        public int Id                       { get; protected set; } = 0;

        public int CardInstanceId           { get; set; } = 0;
        public CardInstance Instance        { get; set; } = null;

        public ReviewValues.Values Value    { get; set; } = ReviewValues.Values.VALUE_0;

        public DateTimeOffset? DateTime     { get; set; } = null;


        private CardReview()
        {
            //
        }

        public CardReview(CardInstance instance)
        {
            this.Instance = instance;
        }
    }
}
