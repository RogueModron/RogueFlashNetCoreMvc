namespace RogueFlashNetCoreMvc.Model.Unmapped
{
    public class FindDecksResult
    {
        public int DeckId                   { get; set; } = 0;
        public string Description           { get; set; } = "";
        public string Notes                 { get; set; } = "";
        public int NumberOfSides            { get; set; } = 0;
    }
}
