namespace JobHunting.Areas.Candidates.ViewModels
{
    public class NotificationSendReplyInputViewModel
    {
        public int CandidateId { get; set; }

        public int NotificationId { get; set; }

        public string Reply {  get; set; }
    }
}
