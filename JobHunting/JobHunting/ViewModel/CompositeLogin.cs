namespace JobHunting.ViewModel
{
    public class CompositeLogin
    {
        public string Role {  get; set; }
        public CandidateLoginVM CandidateLoginVM { get; set; }
        public CompanyLoginVM CompanyLoginVM { get; set; }
    }
}
