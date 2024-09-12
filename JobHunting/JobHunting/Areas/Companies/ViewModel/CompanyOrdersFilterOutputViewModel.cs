namespace JobHunting.Areas.Companies.ViewModel
{
    public class CompanyOrdersFilterOutputViewModel
    {
        public int OrderId { get; set; }

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? PayDate { get; set; }

        public int Duration { get; set; }

        public bool Status { get; set; }
    }
}
