namespace JobHunting.Areas.Companies.ViewModel
{
    public class PutCompanyIntroOutput
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }
        public string CompanyClassId { get; set; }
        public string Address { get; set; }
        public string? Intro { get; set; }

        public string? Benefits { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }
        public string? CompanyClassName { get; set; }
    }
}
