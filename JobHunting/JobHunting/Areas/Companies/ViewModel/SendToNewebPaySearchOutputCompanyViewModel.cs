namespace JobHunting.Areas.Companies.ViewModel
{
    public class SendToNewebPaySearchOutputCompanyViewModel
    {
        public string OrderId { get; set; }

        public int Price { get; set; }

        public string Title { get; set; }

        public string Orderdate { get; set; }

        public string? PayDate { get; set; }

        public int Duration { get; set; }

        public string NewebPayStatus { get; set; }

        public string NewebPayMessage { get; set; }

        public string TradeNo { get; set; }

        public string PaymentType { get; set; }


    }
}
