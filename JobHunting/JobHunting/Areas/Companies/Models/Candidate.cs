﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Companies.Models;

public partial class Candidate
{
    public int CandidateID { get; set; }

    public string NationalID { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public bool? Sex { get; set; }

    public DateOnly? Birthday { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }

    public string Degree { get; set; }

    public string EmploymentStatus { get; set; }

    public string MilitaryService { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<OpinionLetter> OpinionLetters { get; set; } = new List<OpinionLetter>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}