namespace JobHunting.Areas.Companies.ViewModel
{
    public class OpinionLetterOutputModel
    {
        public int LetterId { get; set; }

        public int? CompanyId { get; set; }

        public string Class { get; set; }

        public string SubjectLine { get; set; }

        public bool Status { get; set; }

        public DateTime SendTime { get; set; }
    }
}
