namespace JobHunting.Areas.Companies.ViewModel
{
    public class CompanyOrdersFilterInputViewModel
    {
        public string OrderId { get; set; }

        public int OrderNumber { get; set; }

        public int? CompanyId { get; set; }

        public string Title { get; set; }

        public decimal? Price { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? PayDate { get; set; }

        public int Duration { get; set; }

        public bool Status { get; set; }

        public string StatusType { get; set; }

        public string? NewebPayStatus { get; set; }
    }
}
