namespace JobHunting.ViewModel
{
    public class ResumeInputModel
    {
        public int Perpage { get; set; } 
        public int CurrentPage { get; set; }
        public int? Skill { get; set; }
        public string? Edu { get; set; }
        public string Area { get; set; }
        public string serchText { get; set; }
        public bool? Sex { get; set; }
        public string? zipCode { get; set; }
    }
}
