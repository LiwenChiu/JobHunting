﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Candidates.Models;

public partial class AdminRecord
{
    public int RecordId { get; set; }

    public int AdminId { get; set; }

    public string Task { get; set; }

    public string CRUD { get; set; }

    public DateTime Time { get; set; }
}