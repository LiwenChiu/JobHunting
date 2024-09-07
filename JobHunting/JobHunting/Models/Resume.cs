using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class Resume
{
    public int ResumeId { get; set; }

    public int CandidateId { get; set; }

    public string Title { get; set; } = null!;

    public string? TitleClassId { get; set; }

    public string? Intro { get; set; }

    public string? Autobiography { get; set; }

    public string? WorkExperience { get; set; }

    public byte[]? Certification { get; set; }

    public string? Time { get; set; }

    public string? Address { get; set; }

    public bool ReleaseYn { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual ICollection<CompanyResumeRecord> CompanyResumeRecords { get; set; } = new List<CompanyResumeRecord>();

    public virtual ICollection<ResumeOpeningRecord> ResumeOpeningRecords { get; set; } = new List<ResumeOpeningRecord>();

    public virtual TitleClass? TitleClass { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
