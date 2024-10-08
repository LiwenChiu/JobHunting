﻿namespace JobHunting.Areas.Companies.ViewModel
{
    public class ResumeStorageInputModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public bool? Sex { get; set; }

        public DateOnly? Birthday { get; set; }
        public string? EmploymentStatus { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public int CompanyId { get; set; }

        public string Degree { get; set; }
        public string? Time { get; set; }
        public string Title { get; set; }
        public string OpeningTitle { get; set; }
        public byte[] Headshot { get; set; }
        public byte[] Certification { get; set; }
        public string? WorkExperience { get; set; }
        public string? Autobiography { get; set; }
        public int CandidateId { get; set; }
        public int ResumeId { get; set; }
        public int OpeningId { get; set; }
        public bool ReleaseYN { get; set; }
        public string? Intro { get; set; }
        public List<int> TagId { get; set; }
        public List<int> TitleClassId { get; set; }

        public List<IFormFile>? CertificationImageFile { get; set; }
        public IFormFile? HeadshotImageFile { get; set; }
    }
}
