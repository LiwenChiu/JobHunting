using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class CompanyOrdersFilterViewModel
    {
        [Key]
        public int OrderId { get; set; }

        public int? CompanyId { get; set; }

        public int? PlanId { get; set; }

        public string CompanyName { get; set; }

        public string GUINumber { get; set; }

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public string PayDate { get; set; }

        public int Duration { get; set; }

        public string Expiration { get; set; }

        public string? Intro { get; set; }

        public bool Status { get; set; }
    }
}
