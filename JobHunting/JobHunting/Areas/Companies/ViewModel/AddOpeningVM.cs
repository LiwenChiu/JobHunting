using JobHunting.Areas.Companies.Models;

namespace JobHunting.Areas.Companies.ViewModel
{
    public class AddOpeningVM
    {

        public string Title { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Benefits { get; set; }

        public decimal? SalaryMax { get; set; }

        public decimal? SalaryMin { get; set; }

        public string Time { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public virtual Company Company { get; set; }

    }
}
