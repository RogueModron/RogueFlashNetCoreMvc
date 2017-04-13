namespace RogueFlashNetCoreMvc.ViewData
{
    public class CardData
    {
        public int DeckId           { get; set; } = 0;
        public int CardId           { get; set; } = 0;
        public string SideA         { get; set; } = "";
        public string SideB         { get; set; } = "";
        public string Notes         { get; set; } = "";
        public string Tags          { get; set; } = "";
        public bool SideBToA        { get; set; } = false;
    }
}
