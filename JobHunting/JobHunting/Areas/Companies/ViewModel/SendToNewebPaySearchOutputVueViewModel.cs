namespace JobHunting.Areas.Companies.ViewModel
{
    public class SendToNewebPaySearchOutputVueViewModel
    {
        public bool Status { get; set; } // API是否成功

        public bool? SearchStatus { get; set; } //true: 訂單已回傳金流狀態(不管是不是SUCCESS)，不用去藍新金流查;

        public SendToNewebPaySearchOutputCompanyViewModel? OrderData { get; set; } = new SendToNewebPaySearchOutputCompanyViewModel();

        public SendToNewebPaySearchOutViewModel? outModelreturnObj { get; set; } = new SendToNewebPaySearchOutViewModel();
    }
}
