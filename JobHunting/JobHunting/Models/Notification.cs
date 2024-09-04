﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class Notification
{
    public int NotificationID { get; set; }

    public int? CompanyID { get; set; }

    public int? CandidateID { get; set; }

    public int? ResumeID { get; set; }

    public int? OpeningID { get; set; }

    public string Status { get; set; }

    public string SubjectLine { get; set; }

    public string Content { get; set; }

    public DateOnly SendDate { get; set; }

    public DateTime? AppointmentTime { get; set; }

    public virtual Candidate Candidate { get; set; }

    public virtual Company Company { get; set; }
}