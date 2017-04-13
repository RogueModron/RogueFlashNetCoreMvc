using System.Collections.Generic;
using System.Linq;

namespace EF6Scratch.Model
{
    public class Card
    {
        public int Id { get; protected set; } = 0;
        public int Version { get; set; } = 0;

        public int DeckId { get; set; } = 0;
        public Deck Deck { get; set; } = null;

        public string SideA { get; set; } = "";
        public string SideB { get; set; } = "";
        public string Notes { get; set; } = "";
        public string Tags { get; set; } = "";

        public IList<CardInstance> Instances { get; set; } = new List<CardInstance>();


        private Card()
        {
            //
        }

        public Card(
                Deck deck,
                string sideA,
                string sideB)
        {
            this.Deck = deck;

            this.SideA = sideA;
            this.SideB = sideB;

            var instanceAToB = new CardInstance(this);
            instanceAToB.SideAToB = true;
            this.Instances.Add(instanceAToB);
            var instanceBToA = new CardInstance(this);
            instanceBToA.SideAToB = false;
            this.Instances.Add(instanceBToA);
        }


        public CardInstance GetInstanceSideAToB()
        {
            return FindInstace(true);
        }

        public CardInstance GetInstanceSideBToA()
        {
            return FindInstace(false);
        }


        private CardInstance FindInstace(bool sideAToB)
        {
            return Instances.First(i => i.SideAToB == sideAToB);
        }
    }
}
