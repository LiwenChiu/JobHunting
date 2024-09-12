namespace JobHunting.Areas.Companies.ViewModel
{
    public class PayAgreementPricingPlanViewModel
    {
        public int PlanId { get; set; }

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public decimal? Discount { get; set; }

        public int Duration { get; set; }
    }
}
