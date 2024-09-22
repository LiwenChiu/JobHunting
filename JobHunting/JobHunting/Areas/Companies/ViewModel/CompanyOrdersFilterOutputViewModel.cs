namespace JobHunting.Areas.Companies.ViewModel
{
    public class CompanyOrdersFilterOutputViewModel
    {
        public int OrderId { get; set; }

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public string OrderDate { get; set; }

        public string? PayDate { get; set; }

        public int Duration { get; set; }

        public bool Status { get; set; }
    }
}
