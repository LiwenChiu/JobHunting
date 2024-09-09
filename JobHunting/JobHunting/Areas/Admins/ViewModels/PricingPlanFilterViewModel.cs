using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class PricingPlanFilterViewModel
    {
        [Key]
        public int planId { get; set; }

        public string title { get; set; }

        public string intro { get; set; }

        public int duration { get; set; }

        public decimal? price { get; set; }

        public decimal? discount { get; set; }

        public bool status { get; set; }
    }
}
