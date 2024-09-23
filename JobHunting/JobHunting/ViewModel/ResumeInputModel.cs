namespace JobHunting.ViewModel
{
    public class ResumeInputModel
    {
        public int Perpage { get; set; } 
        public int CurrentPage { get; set; }
        public int? Skill { get; set; }
        public string? Edu { get; set; }
        public string? AreaName { get; set; }
        public string? searchText { get; set; }
        public bool? Sex { get; set; }
    }
}
