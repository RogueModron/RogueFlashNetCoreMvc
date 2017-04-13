using System.Collections.Generic;

namespace RogueFlashNetCoreMvc.Model
{
    public class Deck
    {
        public int Id                       { get; protected set; } = 0;
        public int Version                  { get; set; } = 0;

        public string Description           { get; set; } = "";
        public string Notes                 { get; set; } = "";

        public IList<Card> Cards            { get; set; } = new List<Card>();


        private Deck()
        {
            //
        }

        public Deck(string description)
        {
            this.Description = description;
        }


        public Card AddCard()
        {
            var card = new Card(this, "", "");
            GetOrInitializaCards().Add(card);
            return card;
        }

        private IList<Card> GetOrInitializaCards()
        {
            if (Cards == null)
            {
                Cards = new List<Card>();
            }
            return Cards;
        }
    }
}
