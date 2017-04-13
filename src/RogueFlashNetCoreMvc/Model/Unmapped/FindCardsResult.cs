namespace RogueFlashNetCoreMvc.Model.Unmapped
{
    public class FindCardsResult
    {
        public int CardId                   { get; set; } = 0;
        public string SideA                 { get; set; } = "";
        public string SideB                 { get; set; } = "";
        public string Notes                 { get; set; } = "";
        public string Tags                  { get; set; } = "";
        public bool SideAToBDisabled        { get; set; } = false;
    }
}
