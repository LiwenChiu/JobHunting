using JobHunting.Areas.Candidates.Models;

namespace JobHunting.Areas.Candidates.ViewModels
{
    public class ResumeInputModel
    {
        public string Address { get; set; }



        public string EmploymentStatus { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string WorkExperience { get; set; }
        public string Autobiography { get; set; }
        public int CandidateId { get; set; }
        public int ResumeId { get; set; }
        public bool ReleaseYN { get; set; }
        public string Intro { get; set; }
        public List<int> TagId { get; set; }
        public List<int> TitleClassId { get; set; }

        public string TitleClassName { get; set; }

        public IFormFile? CertificationImageFile { get; set; }
        public IFormFile? HeadshotImageFile { get; set; }
    }
}
