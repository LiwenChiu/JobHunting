namespace JobHunting.Areas.Companies.ViewModel
{
    public class CompanyInsertLetter
    {
        public int CompanyId { get; set; }
        public string Letterclass { get; set; }
        public string SubjectLine { get; set; }
        public string Content { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateTime SendTime { get; set; }
    }
}
