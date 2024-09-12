namespace JobHunting.Areas.Candidates.ViewModels
{
    public class addResumeInputModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public bool? Sex { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Degree { get; set; }
        public string Email { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? Time { get; set; }
        public string Title { get; set; }
        public byte[]? Certification { get; set; }
        public string? WorkExperience { get; set; }
        public string? Autobiography { get; set; }
        public int CandidateId { get; set; }
        public int ResumeId { get; set; }
        public bool ReleaseYN { get; set; }
        public string? Intro { get; set; }

        //public byte[]? Headshot { get; set; }
        public string TitleClassName { get; set; }

        public IFormFile? ImageFile { get; set; }


    }
}
