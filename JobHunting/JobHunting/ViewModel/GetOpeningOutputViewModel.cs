using JobHunting.Areas.Admins.Models;

namespace JobHunting.ViewModel
{
    public class GetOpeningOutputViewModel
    {
        public string OpeningTitle { get; set; }

        public string CompanyName { get; set; }

        public string? Address { get; set; }

        public int? RequiredNumber { get; set; }

        public int? ResumeNumber { get; set; }

        public string? Description { get; set; }

        public string? Benefit { get; set; }

        public string Degree { get; set; }

        public bool InterviewYN { get; set; }

        public decimal?  SalaryMax { get; set; }

        public decimal? SalaryMin { get; set; }

        public string? Time { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public IEnumerable<string>? TitleClassName { get; set; }

        public IEnumerable<string>? TagName { get; set;}
    }
}
