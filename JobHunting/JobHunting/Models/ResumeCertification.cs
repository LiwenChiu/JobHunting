﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class ResumeCertification
{
    public int CertificationId { get; set; }

    public int ResumeId { get; set; }

    public string CertificationName { get; set; }

    public byte[] FileData { get; set; }

    public string ContentType { get; set; }

    public virtual Resume Resume { get; set; }
}