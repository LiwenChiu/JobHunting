﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Candidates.Models;

public partial class CompanyResumeRecord
{
    public int CompanyID { get; set; }

    public int ResumeID { get; set; }

    public bool LikeYN { get; set; }

    public bool? InterviewYN { get; set; }

    public bool? HireYN { get; set; }

    public virtual Company Company { get; set; }

    public virtual Resume Resume { get; set; }
}