﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class JobParameter
{
    public long JobId { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public virtual Job Job { get; set; }
}