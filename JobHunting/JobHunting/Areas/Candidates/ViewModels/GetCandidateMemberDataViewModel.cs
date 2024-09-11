namespace JobHunting.Areas.Candidates.ViewModels
{
    public class GetCandidateMemberDataViewModel
    {
        public string Name { get; set; }

        public byte[] Headshot { get; set; }

        public string? TitleClassName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string EmploymentStatus { get; set; }
    }
}
