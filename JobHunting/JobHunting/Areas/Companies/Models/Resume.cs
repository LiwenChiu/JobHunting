﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Companies.Models;

public partial class Resume
{
    public int ResumeID { get; set; }

    public int CandidateID { get; set; }

    public string Title { get; set; }

    public string TitleClassID { get; set; }

    public string Intro { get; set; }

    public string Autobiography { get; set; }

    public string WorkExperience { get; set; }

    public byte[] Certification { get; set; }

    public string Time { get; set; }

    public string Address { get; set; }

    public bool ReleaseYN { get; set; }

    public virtual Candidate Candidate { get; set; }

    public virtual ICollection<CompanyResumeRecord> CompanyResumeRecords { get; set; } = new List<CompanyResumeRecord>();

    public virtual ICollection<ResumeOpeningRecord> ResumeOpeningRecords { get; set; } = new List<ResumeOpeningRecord>();

    public virtual TitleClass TitleClass { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}