using JobHunting.Areas.Companies.Models;
using JobHunting.Models;
using System.ComponentModel;

namespace JobHunting.Areas.Companies.ViewModel
{
    public class ReceiveResumeViewModel
    {
        [DisplayName("應徵日期")]
        public DateOnly? ApplyDate { get; set; }
        [DisplayName("職缺名稱")]
        public string Title { get; set; }
        [DisplayName("職缺類別")]
        public string TitleClassName { get; set; }
    }
}
