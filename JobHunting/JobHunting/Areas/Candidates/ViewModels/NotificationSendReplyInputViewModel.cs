namespace JobHunting.Areas.Candidates.ViewModels
{
    public class NotificationSendReplyInputViewModel
    {

        public int NotificationId { get; set; }

        public string Reply {  get; set; }

        public bool InterviewYN { get; set; }

        public bool HireYN { get; set; }
        public int ResumeOpeningRecordId { get; set; }
    }
}
