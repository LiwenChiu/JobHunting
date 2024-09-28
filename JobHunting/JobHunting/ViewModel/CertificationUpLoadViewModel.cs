namespace JobHunting.ViewModel
{
    public class CertificationUpLoadViewModel
    {
        public int ResumeId { get; set; }

        public string? CertificationName { get; set; }

        public IFormFile? FileData { get; set; }

        public string? ContentType { get; set; }
    }
}
