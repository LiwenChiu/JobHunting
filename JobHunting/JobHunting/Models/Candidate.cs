﻿using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string NationalId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public bool? Sex { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Degree { get; set; }

    public string? EmploymentStatus { get; set; }

    public string? MilitaryService { get; set; }

    public byte[]? Headshot { get; set; }


    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<OpinionLetter> OpinionLetters { get; set; } = new List<OpinionLetter>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}
