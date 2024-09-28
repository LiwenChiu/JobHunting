using JobHunting.Areas.Companies.Models;
using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using System.ComponentModel;

namespace JobHunting.Areas.Companies.ViewModel
{
    public class ReceiveResumeOutputModel
    {
        [DisplayName("應徵日期")]
        public DateOnly? ApplyDate { get; set; }
        [DisplayName("職缺名稱")]
        public string Title { get; set; }
        [DisplayName("職缺類別")]
        public string TitleClassName { get; set; }


        public List<int> TitleCategoryId { get; set; }
        public int ResumeOpeningRecordId { get; set; }

        public int? ResumeId { get; set; }

        public int? OpeningId { get; set; }

        public string OpeningTitle { get; set; }

        public int? CompanyId { get; set; }

        public string CompanyName { get; set; }


        public bool? InterviewYN { get; set; }

        public bool? HireYN { get; set; }


        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public bool? Sex { get; set; }

        public DateOnly? Birthday { get; set; }
        public string? EmploymentStatus { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }


        public string Degree { get; set; }
        public string? Time { get; set; }

        public byte[] Headshot { get; set; }
        public byte[] Certification { get; set; }
        public string? WorkExperience { get; set; }
        public string? Autobiography { get; set; }
        public int CandidateId { get; set; }
        public bool ReleaseYN { get; set; }
        public string? Intro { get; set; }
        public List<int> TagId { get; set; }
        public List<int> TitleClassId { get; set; }

        public List<IFormFile>? CertificationImageFile { get; set; }
        public IFormFile? HeadshotImageFile { get; set; }

    }
}
