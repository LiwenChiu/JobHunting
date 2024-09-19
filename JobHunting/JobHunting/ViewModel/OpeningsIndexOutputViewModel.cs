namespace JobHunting.ViewModel
{
    public class OpeningsIndexOutputViewModel
    {
        public int totalDataCount { get; set; }

        public IEnumerable<OpeningsIndexViewModel> OpeningsIndexOutput { get; set; }
    }
}
