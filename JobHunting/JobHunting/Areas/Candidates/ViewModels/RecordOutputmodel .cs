using JobHunting.Areas.Candidates.Models;
using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Candidates.ViewModels
{
    public class RecordOutputmodel
    {
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; }
        [Display(Name = "應徵日期")]

        public DateOnly? ApplyDate { get; set; }
        [Display(Name = "職缺")]

        public string OpeningTitle { get; set; }
        [Display(Name = "履歷")]
        public string Title { get; set; }
        public int ResumeOpeningRecordID { get; set; }
    }
}
