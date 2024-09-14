namespace JobHunting.Areas.Companies.ViewModel
{
    public class NotificationCompanyLessOutputViewModel
    {
        public int NotificationId { get; set; }

        public int? CompanyId { get; set; }

        public int? CandidateId { get; set; }

        public int? OpeningId { get; set; }

        public int? ResumeId { get; set; }

        public string Status { get; set; }

        public string SubjectLine { get; set; }

        public string Content { get; set; }

        public DateOnly SendDate { get; set; }

        public DateTime? AppointmentTime { get; set; }

        public string CompanyName { get; set; }

        public string CandidateName { get; set; }
    }
}
