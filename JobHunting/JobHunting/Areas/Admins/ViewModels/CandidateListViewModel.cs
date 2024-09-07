namespace JobHunting.Areas.Admins.ViewModels
{
    public class CandidateListViewModel
    {
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public bool? Sex { get; set; }
        public DateOnly? Birthday { get; set; }
        public string Degree { get; set; }
        public string Address { get; set; }

    }
}
