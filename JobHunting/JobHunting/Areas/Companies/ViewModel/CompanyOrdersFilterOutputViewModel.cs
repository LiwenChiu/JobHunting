namespace JobHunting.Areas.Companies.ViewModel
{
    public class CompanyOrdersFilterOutputViewModel
    {
        public string OrderId { get; set; }

        public int OrderNumber { get; set; }

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public string OrderDate { get; set; }

        public string? PayDate { get; set; }

        public int Duration { get; set; }

        public bool Status { get; set; }

        public string StatusType { get; set; }
    }
}
