﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Admins.Models;

public partial class OpinionLetter
{
    public int LetterID { get; set; }

    public int? CompanyID { get; set; }

    public int? CandidateID { get; set; }

    public int? AdminID { get; set; }

    public string Class { get; set; }

    public string SubjectLine { get; set; }

    public string Content { get; set; }

    public byte[] Attachment { get; set; }

    public bool Status { get; set; }

    public virtual Admin Admin { get; set; }

    public virtual Candidate Candidate { get; set; }

    public virtual Company Company { get; set; }
}