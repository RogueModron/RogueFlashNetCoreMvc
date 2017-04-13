using System.Collections.Generic;

namespace RogueFlashNetCoreMvc.Model
{
    public class CardInstance
    {
        public int Id                       { get; protected set; } = 0;

        public int CardId                   { get; set; } = 0;
        public Card Card                    { get; set; } = null;

        public bool SideAToB                { get; set; } = false;
        public bool Disabled                { get; set; } = false;

        public CardPlan Plan                { get; set; } = null;

        public IList<CardReview> Reviews    { get; set; } = new List<CardReview>();


        private CardInstance()
        {
            //
        }

        public CardInstance(Card card)
        {
            this.Card = card;

            this.Plan = new CardPlan(this);
        }


        public CardReview AddReview()
        {
            var review = new CardReview(this);
            Reviews.Add(review);
            return review;
        }
    }
}
