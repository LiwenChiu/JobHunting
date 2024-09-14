namespace JobHunting.Areas.Companies.ViewModel
{
    public class NotificationCompanyFullOutputViewModel
    {
        public IEnumerable<NotificationCompanyLessOutputViewModel> companyNotificationsFilter { get; set; } = new List<NotificationCompanyLessOutputViewModel>();

        public int totalDataCount { get; set; }
    }
}
