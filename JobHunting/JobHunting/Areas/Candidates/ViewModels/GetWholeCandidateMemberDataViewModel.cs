﻿namespace JobHunting.Areas.Candidates.ViewModels
{
    public class GetWholeCandidateMemberDataViewModel
    {
        public int CandidateId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool? Sex { get; set; }

        public DateOnly? Birthday { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string? Degree { get; set; }

        public string? EmploymentStatus { get; set; }

        public string? MilitaryService { get; set; }
    }
}
