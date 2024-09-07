using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string Guinumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? TitleClassId { get; set; }

    public string? Address { get; set; }

    public string? Intro { get; set; }

    public string? Benefits { get; set; }

    public byte[]? Picture { get; set; }

    public string ContactName { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public string ContactEmail { get; set; } = null!;

    public bool Status { get; set; }

    public DateTime Date { get; set; }

    public virtual ICollection<CompanyOrder> CompanyOrders { get; set; } = new List<CompanyOrder>();

    public virtual ICollection<CompanyResumeRecord> CompanyResumeRecords { get; set; } = new List<CompanyResumeRecord>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Opening> Openings { get; set; } = new List<Opening>();

    public virtual ICollection<OpinionLetter> OpinionLetters { get; set; } = new List<OpinionLetter>();

    public virtual TitleClass? TitleClass { get; set; }
}
