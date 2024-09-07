using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class PricingPlanIntroViewModel
    {
        [Key]
        public int PlanId { get; set; }

        public string Intro { get; set; }
    }
}
