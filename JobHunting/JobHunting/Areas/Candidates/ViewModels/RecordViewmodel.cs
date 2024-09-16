using JobHunting.Areas.Candidates.Models;
using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Candidates.ViewModels
{
    public class RecordViewmodel
    {
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; }
        [Display(Name = "應徵日期")]

        public string ApplyDate { get; set; }
        [Display(Name = "職缺")]

        public string OpeningTitle { get; set; }

        [Display(Name = "履歷")]
        public string Title { get; set; }

        //public int ResumeOpeningRecordID { get; set; }

        //public int ResumeId { get; set; }

        //public int OpeningId { get; set; }

        public int CandidateId { get; set; }
    }
}
