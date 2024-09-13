namespace JobHunting.Areas.Admins.ViewModels
{
    public class CompanyOrdersOutputViewModel
    {
        public int totalPage { get; set; }

        public IEnumerable<CompanyOrdersFilterViewModel> companyOrdersFilter { get; set; } = new List<CompanyOrdersFilterViewModel>();
    }
}
