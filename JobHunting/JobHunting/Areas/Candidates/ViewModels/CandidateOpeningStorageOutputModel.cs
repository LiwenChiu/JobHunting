namespace JobHunting.Areas.Candidates.ViewModels
{
    public class CandidateOpeningStorageOutputModel
    {
        public int? OpeningId { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public int? RequiredNumber { get; set; }

        public int? ResumeNumber { get; set; }

        public string Description { get; set; }

        public string Degree { get; set; }

        public string Benefits { get; set; }

        public decimal? SalaryMax { get; set; }

        public decimal? SalaryMin { get; set; }

        public string Time { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string CompanyName {  get; set; }

        public List<int> TitleClassId { get; set; }

        public List<int> TagId { get; set; }

        public bool ReleaseYN {  get; set; }
    }
}
