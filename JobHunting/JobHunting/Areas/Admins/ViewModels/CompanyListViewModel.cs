using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class CompanyListViewModel
    {
        [Display(Name = "公司ID")]
        public int CompanyId { get; set; }

        [Display(Name = "公司名稱")]

        public string CompanyName { get; set; }

        [Display(Name = "統一編號")]
        public string GUINumber { get; set; }

        [Display(Name = "聯絡人")]
        public string ContactName { get; set; }

        [Display(Name = "電話")]
        public string ContactPhone { get; set; }

        [Display(Name = "電子信箱")]
        public string ContactEmail { get; set; }

    }
}
