namespace JobHunting.ViewModel
{
    public class AddInterviewViewModel
    {
        public int? CompanyId { get; set; }

        public string? CandidateId { get; set; }

        public int OpeningId { get; set; }

        public string? ResumeId { get; set; }
        public string Status { get; set; }
        public string SubjectLine { get; set; }

        public string Content { get; set; }
        public string Address { get; set; }
        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly? AppointmentTime { get; set; }

        public bool InterviewYN { get; set; }

        public bool HireYN { get; set; }
        public bool Title { get; set; }
        public string OpeningTitle { get; set; }
        public string CompanyName { get; set; }
    }
}
