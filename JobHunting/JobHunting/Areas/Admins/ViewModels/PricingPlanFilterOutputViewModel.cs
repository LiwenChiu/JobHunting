namespace JobHunting.Areas.Admins.ViewModels
{
    public class PricingPlanFilterOutputViewModel
    {
        public int planId { get; set; }

        public string title { get; set; }

        public string intro { get; set; }

        public int duration { get; set; }

        public decimal? price { get; set; }

        public decimal? discount { get; set; }

        public bool status { get; set; }

        public bool edit {  get; set; }
    }
}
