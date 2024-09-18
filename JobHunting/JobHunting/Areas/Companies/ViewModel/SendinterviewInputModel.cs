namespace JobHunting.Areas.Companies.ViewModel
{
    public class SendinterviewInputModel
    {
        public int NotificationId { get; set; }

        public int? CandidateId { get; set; }

        public int? OpeningId { get; set; }

        public int? ResumeId { get; set; }

        public string Status { get; set; }

        public string SubjectLine { get; set; }

        public string Content { get; set; }

        public DateOnly SendDate { get; set; }

        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly? AppointmentTime { get; set; }

        public string Address { get; set; }
        public bool ReplyYN { get; set; }
        public bool ReplyFirstYN { get; set; }

    }
}
