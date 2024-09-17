﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Admins.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? CompanyId { get; set; }

    public int? CandidateId { get; set; }

    public int? OpeningId { get; set; }

    public int? ResumeId { get; set; }

    public string Status { get; set; }

    public string SubjectLine { get; set; }

    public string Content { get; set; }

    public DateOnly SendDate { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public TimeOnly? AppointmentTime { get; set; }

    public string Address { get; set; }

    public bool ReplyFirstYN { get; set; }

    public bool ReplyYN { get; set; }

    public string Reply { get; set; }

    public DateTime? ReplyTime { get; set; }

    public virtual Candidate Candidate { get; set; }

    public virtual Company Company { get; set; }
}