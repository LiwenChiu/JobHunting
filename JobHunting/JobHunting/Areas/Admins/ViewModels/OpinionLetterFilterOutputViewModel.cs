namespace JobHunting.Areas.Admins.ViewModels
{
    public class OpinionLetterFilterOutputViewModel
    {
        public int LetterId { get; set; }

        public string? Class { get; set; }

        public string? SubjectLine { get; set; }

        public string SendTime { get; set; }

        public bool Status { get; set; }
    }
}
