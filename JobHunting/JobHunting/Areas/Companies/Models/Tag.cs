﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Companies.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public int? TagClassId { get; set; }

    public string TagName { get; set; }

    public virtual TagClass TagClass { get; set; }

    public virtual ICollection<Opening> Openings { get; set; } = new List<Opening>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}