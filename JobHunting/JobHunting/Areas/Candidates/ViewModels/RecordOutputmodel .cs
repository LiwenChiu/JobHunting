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

        public int? ResumeId { get; set; }

        public int? OpeningId { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Degree { get; set; }

        public string Benefits { get; set; }

        public decimal? SalaryMax { get; set; }

        public decimal? SalaryMin { get; set; }

        public string Time { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public List<int> TitleClassId { get; set; }

        public List<int> TagId { get; set; }

        public int CandidateId { get; set; }

        public bool InterviewYN { get; set; }

        public bool HireYN { get; set; }
    }
}
