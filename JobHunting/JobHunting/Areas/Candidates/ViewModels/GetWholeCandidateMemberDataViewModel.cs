namespace JobHunting.Areas.Candidates.ViewModels
{
    public class GetWholeCandidateMemberDataViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string? Sex { get; set; }

        public DateOnly? Birthday { get; set; }

        public int? TitleClassId { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Degree { get; set; }

        public string EmploymentStatus { get; set; }

        public string MilitaryService { get; set; }
    }
}
