﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Companies.Models;

public partial class Resume
{
    public int ResumeId { get; set; }

    public int CandidateId { get; set; }

    public string Title { get; set; }

    public string Intro { get; set; }

    public byte[] Headshot { get; set; }

    public string Autobiography { get; set; }

    public string WorkExperience { get; set; }

    public string Time { get; set; }

    public string Address { get; set; }

    public bool ReleaseYN { get; set; }

    public DateTime LastEditTime { get; set; }

    public virtual Candidate Candidate { get; set; }

    public virtual ICollection<ResumeCertification> ResumeCertifications { get; set; } = new List<ResumeCertification>();

    public virtual ICollection<ResumeOpeningRecord> ResumeOpeningRecords { get; set; } = new List<ResumeOpeningRecord>();

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<TitleClass> TitleClasses { get; set; } = new List<TitleClass>();
}