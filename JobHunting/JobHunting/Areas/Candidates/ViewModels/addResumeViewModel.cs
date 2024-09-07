﻿using JobHunting.Areas.Candidates.Models;

namespace JobHunting.Areas.Candidates.ViewModels
{
    public class addResumeViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? Sex { get; set; }
        public DateOnly? Birthday { get; set; }
        public string Phone { get; set; }
        public string Degree { get; set; }
        public string Email { get; set; }
        public string EmploymentStatus { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string TitleClassID { get; set; }
        public byte[] Certification { get; set; }
        public string WorkExperience { get; set; }
        public string Autobiography { get; set; }
        public int CandidateID { get; set; }
    }
}
