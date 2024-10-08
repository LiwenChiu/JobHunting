﻿namespace JobHunting.Areas.Companies.ViewModel
{
    public class NotificationCompanyLessOutputViewModel
    {
        public int NotificationId { get; set; }

        public int? OpeningId { get; set; }

        public int? ResumeId { get; set; }

        public string Status { get; set; }

        public string SubjectLine { get; set; }

        public DateOnly? AppointmentDate { get; set; }

        public bool ReplyYN { get; set; }

        public string CandidateName { get; set; }
    }
}
