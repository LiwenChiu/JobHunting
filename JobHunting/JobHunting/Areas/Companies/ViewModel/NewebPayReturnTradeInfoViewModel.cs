namespace JobHunting.Areas.Companies.ViewModel
{
    public class NewebPayReturnTradeInfoViewModel
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public string MerchantID { get; set; }

        public string Amt { get; set; }

        public string TradeNo { get; set; }

        public string MerchantOrderNo { get; set; }

        public string PaymentType { get; set; }

        public DateTime? PayTime { get; set; }

        public string IP { get; set; }

        public string EscrowBank { get; set; }
    }
}
