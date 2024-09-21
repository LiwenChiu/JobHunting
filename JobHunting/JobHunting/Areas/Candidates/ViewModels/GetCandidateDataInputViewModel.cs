namespace JobHunting.Areas.Candidates.ViewModels
{
    public class GetCandidateDataInputViewModel
    {
        public string? Name { get; set; }

        public bool? Sex { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Degree { get; set; }

        public bool VerifyEmailYN { get; set; }
    }
}
