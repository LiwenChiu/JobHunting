﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class TitleClass
{
    public int TitleClassId { get; set; }

    public int? TitleCategoryId { get; set; }

    public string TitleClassName { get; set; }

    public virtual TitleCategory TitleCategory { get; set; }

    public virtual ICollection<Opening> Openings { get; set; } = new List<Opening>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}