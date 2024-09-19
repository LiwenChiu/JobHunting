﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Admins.Models;

public partial class Opening
{
    public int OpeningId { get; set; }

    public int CompanyId { get; set; }

    public string Title { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    public string Degree { get; set; }

    public string Benefits { get; set; }

    public bool InterviewYN { get; set; }

    public decimal? SalaryMax { get; set; }

    public decimal? SalaryMin { get; set; }

    public string Time { get; set; }

    public string ContactName { get; set; }

    public string ContactPhone { get; set; }

    public string ContactEmail { get; set; }

    public bool ReleaseYN { get; set; }

    public int? RequiredNumber { get; set; }

    public int? ResumeNumber { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<ResumeOpeningRecord> ResumeOpeningRecords { get; set; } = new List<ResumeOpeningRecord>();

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<TitleClass> TitleClasses { get; set; } = new List<TitleClass>();
}