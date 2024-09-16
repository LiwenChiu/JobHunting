namespace JobHunting.ViewModel
{
    public class AddInterviewViewModel
    {
        public int? CompanyId { get; set; }

        public string? CandidateId { get; set; }

        public int? OpeningId { get; set; }

        public string? ResumeId { get; set; }
        public string Status { get; set; }
        public string SubjectLine { get; set; }

        public string Content { get; set; }
        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly? AppointmentTime { get; set; }

    }
}
