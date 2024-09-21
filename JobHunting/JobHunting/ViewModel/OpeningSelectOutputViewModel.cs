namespace JobHunting.ViewModel
{
    public class OpeningSelectOutputViewModel
    {
        
        public int totalDataCount { get; set; }

        public IEnumerable<OpeningSelectViewModel> OpeningsIndexOutput { get; set; }
    }
}
