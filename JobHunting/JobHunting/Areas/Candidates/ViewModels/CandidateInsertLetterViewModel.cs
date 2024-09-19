namespace JobHunting.Areas.Candidates.ViewModels
{
    public class CandidateInsertLetterViewModel
    {
       
        public int CandidateId { get; set; }
        public string Letterclass { get; set; }
        public string SubjectLine { get; set; }
        public string Content { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateTime SendTime { get; set; }
    }
}
