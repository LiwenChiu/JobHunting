﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Companies.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string GUINumber { get; set; }

    public string Password { get; set; }

    public string CompanyName { get; set; }

    public string CompanyClassId { get; set; }

    public string Address { get; set; }

    public string Intro { get; set; }

    public string Benefits { get; set; }

    public byte[] Picture { get; set; }

    public string ContactName { get; set; }

    public string ContactPhone { get; set; }

    public string ContactEmail { get; set; }

    public bool Status { get; set; }

    public DateTime Date { get; set; }

    public DateTime? Deadline { get; set; }

    public virtual CompanyClass CompanyClass { get; set; }

    public virtual ICollection<CompanyOrder> CompanyOrders { get; set; } = new List<CompanyOrder>();

    public virtual ICollection<CompanyResumeRecord> CompanyResumeRecords { get; set; } = new List<CompanyResumeRecord>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Opening> Openings { get; set; } = new List<Opening>();

    public virtual ICollection<OpinionLetter> OpinionLetters { get; set; } = new List<OpinionLetter>();
}