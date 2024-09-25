namespace JobHunting.Areas.Companies.ViewModel
{
    public class SendToNewebPaySearchOutputVueViewModel
    {
        public bool Status { get; set; } // API是否成功

        public bool? SearchStatus { get; set; } //true: 已成功，不用去藍新金流查

        public SendToNewebPaySearchOutputCompanyViewModel? CompanyData { get; set; } = new SendToNewebPaySearchOutputCompanyViewModel();

        public SendToNewebPaySearchOutViewModel? outModelreturnObj { get; set; } = new SendToNewebPaySearchOutViewModel();
    }
}
