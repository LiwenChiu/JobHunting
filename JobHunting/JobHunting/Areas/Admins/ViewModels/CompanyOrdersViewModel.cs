using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class CompanyOrdersViewModel
    {
        [Key]
        public int OrderID { get; set; }

        public int? CompanyID { get; set; }

        public int? PlanID { get; set; }
        
        public string CompanyName { get; set; }

        public string GUINumber { get; set; }

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public DateTime OrderDate { get; set; }

        public int Duration { get; set; }

        public string Intro { get; set; }

        public bool Status { get; set; }
    }
}
