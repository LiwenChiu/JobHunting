namespace JobHunting.Areas.Candidates.ViewModels
{
    public class NotificationCandidateModalOutputViewModel
    {
        public int NotificationId { get; set; }

        public string Status { get; set; }

        public string SubjectLine { get; set; }

        public string Content { get; set; }

        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly? AppointmentTime { get; set; }

        public string Address { get; set; }

        public string CompanyName { get; set; }

        public string CandidateName { get; set; }

        public string OpeningTitle { get; set; }

        public string ResumeTitle { get; set; }

        public bool ReplyFirstYN { get; set; }

        public bool ReplyYN { get; set; }

        public string? Reply { get; set; }

        public DateTime? ReplyTime { get; set; }

        public bool EditReplyYN { get; set; }
    }
}
