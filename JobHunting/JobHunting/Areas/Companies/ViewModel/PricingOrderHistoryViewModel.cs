using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Companies.ViewModel
{
    public class PricingOrderHistoryViewModel
    {
        [Display(Name = "訂單ID")]
        public int OrderID { get; set; }
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; }
        [Display(Name = "統一編號")]
        public int GUINumber { get; set; }
        [Display(Name = "方案名稱")]
        public string PlanTitle { get; set; }
        [Display(Name = "價格")]

        public decimal? Price { get; set; }
        [Display(Name = "訂購日期")]

        public DateTime OrderDate { get; set; }
        [Display(Name = "付款狀態")]

        public bool Status { get; set; }
    }
}
