using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class Opening
{
    public int OpeningId { get; set; }

    public int CompanyId { get; set; }

    public string Title { get; set; } = null!;

    public string? TitleClassId { get; set; }

    public string? Address { get; set; }

    public string Description { get; set; } = null!;

    public string? Degree { get; set; }

    public string? Benefits { get; set; }

    public bool InterviewYn { get; set; }

    public decimal? SalaryMax { get; set; }

    public decimal? SalaryMin { get; set; }

    public string? Time { get; set; }

    public string ContactName { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public string? ContactEmail { get; set; }

    public bool ReleaseYn { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<ResumeOpeningRecord> ResumeOpeningRecords { get; set; } = new List<ResumeOpeningRecord>();

    public virtual TitleClass? TitleClass { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
