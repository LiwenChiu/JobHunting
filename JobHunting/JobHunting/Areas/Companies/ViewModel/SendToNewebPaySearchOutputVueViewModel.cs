namespace JobHunting.Areas.Companies.ViewModel
{
    public class SendToNewebPaySearchOutputVueViewModel
    {
        public bool Status { get; set; } // API是否成功

        public SendToNewebPaySearchOutputCompanyViewModel? OrderData { get; set; } = new SendToNewebPaySearchOutputCompanyViewModel();
    }
}
