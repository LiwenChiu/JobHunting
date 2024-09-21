namespace JobHunting.Areas.Admins.ViewModels
{
    public class EditStatusViewmodel
    {
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public int CompanyId { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }

        public string CompanyName { get; set; }
    }
}
