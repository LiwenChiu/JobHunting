namespace JobHunting.Areas.Candidates.ViewModels
{
    public class OpeningDetail_Outputmodel
    {
        public string CompanyName { get; set; }
        public int OpeningID { get; set; }
        public int CompanyID { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Time { get; set; }
        public decimal? SalaryMax { get; set; }

        public decimal? SalaryMin { get; set; }
        public string Description { get; set; }

        public string Benefits { get; set; }
    }
}
