namespace JobHunting.ViewModel
{
    public class OpeningsUnLoginDataOutputViewModel
    {
        public int totalDataCount { get; set; }

        public IEnumerable<OpeningsUnLoginOutputViewModel> OpeningUnLoginOutput { get; set; }
    }
}
