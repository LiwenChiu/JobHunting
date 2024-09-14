namespace JobHunting.Areas.Companies.ViewModel
{
    public class NotificationsFilterViewModel
    {
        public int CompanyId { get; set; }

        public string? filterInput { get; set; }

        public int pageStart { get; set; }
    }
}
