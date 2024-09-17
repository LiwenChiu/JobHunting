namespace JobHunting.Areas.Candidates.ViewModels
{
    public class NotificationCandidateFullOutputViewModel
    {
        public IEnumerable<NotificationCandidateLessOutputViewModel> candidateNotificationsFilter { get; set; } = new List<NotificationCandidateLessOutputViewModel>();

        public int totalDataCount { get; set; }
    }
}
