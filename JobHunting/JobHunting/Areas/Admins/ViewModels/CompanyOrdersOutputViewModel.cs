namespace JobHunting.Areas.Admins.ViewModels
{
    public class CompanyOrdersOutputViewModel
    {
        public int totalDataCount { get; set; }

        public IEnumerable<CompanyOrdersFilterOutputViewModel> companyOrdersFilter { get; set; } = new List<CompanyOrdersFilterOutputViewModel>();
    }
}
