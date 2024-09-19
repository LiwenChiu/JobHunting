namespace JobHunting.ViewModel
{
    public class ResumesIntroViewModel
    {
        public int ResumeId { get; set; }

        public int CandidateId { get; set; }

        public string Title { get; set; }

        public string Intro { get; set; }

        public string Autobiography { get; set; }

        public string WorkExperience { get; set; }

        public byte[] Certification { get; set; }

        public string WishTime { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public bool? Sex { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public string WishAddress { get; set; }

        public string Degree { get; set; }
        public string EmploymentStatus { get; set; }

        public string MilitaryService { get; set; }
        public object TagObj { get; set; }
        public object TitleObj { get; set; }
        public bool LikeYN { get; set; }
    }
}
