﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class State
{
    public long Id { get; set; }

    public long JobId { get; set; }

    public string Name { get; set; }

    public string Reason { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Data { get; set; }

    public virtual Job Job { get; set; }
}