namespace JobHunting.Areas.Admins.ViewModels
{
    public class CandidateListViewModel
    {
        public int CandidateId { get; set; }
        public string Name { get; set; }
        public bool? Sex { get; set; }
        public DateOnly? Birthday { get; set; }
        public string Degree { get; set; }
        public string Address { get; set; }

        public bool VerifyEmailYN { get; set; }

        public DateTime RegisterTime { get; set; }

        public string Email { get; set; }
    }
}
