namespace JobHunting.Areas.Companies.ViewModel
{
    public class NewebPayTakeNumberTradeInfoViewModel
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public string MerchantID { get; set; }

        public string Amt { get; set; }

        public string TradeNo { get; set; }

        public string MerchantOrderNo { get; set; }

        public string PaymentType { get; set; }

        public DateTime ExpireDate { get; set; }

        //ATM 回傳參數
        public string? BankCode { get; set; }

        public string? ATMCodeNo { get; set; }

        //超商代碼 回傳參數
        public string CodeNo { get; set; }

        public string ExpireTime { get; set; }

        //超商條碼 回傳參數
        public string Barcode_1 { get; set; }

        public string Barcode_2 { get; set; }

        public string Barcode_3 { get; set; }
    }
}
