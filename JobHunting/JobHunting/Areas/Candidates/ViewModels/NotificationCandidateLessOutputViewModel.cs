namespace JobHunting.Areas.Candidates.ViewModels
{
    public class NotificationCandidateLessOutputViewModel
    {
        public int NotificationId { get; set; }

        public int? CompanyId { get; set; }

        public int? OpeningId { get; set; }

        public int? ResumeId { get; set; }

        public string Status { get; set; }

        public string SubjectLine { get; set; }

        public DateOnly? AppointmentDate { get; set; }

        public string CompanyName { get; set; }
    }
}
