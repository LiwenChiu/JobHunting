namespace JobHunting.Areas.Admins.ViewModels
{
    public class PricingPlanCreateViewModel
    {
        public string Title { get; set; }

        public string Intro { get; set; }

        public int Duration { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }
    }
}
