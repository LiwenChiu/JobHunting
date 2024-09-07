using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class PricingPlanIntroViewModel
    {
        [Key]
        public int PlanID { get; set; }

        public string Intro { get; set; }
    }
}
