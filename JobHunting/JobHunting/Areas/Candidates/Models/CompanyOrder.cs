﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Candidates.Models;

public partial class CompanyOrder
{
    public int OrderId { get; set; }

    public int? CompanyId { get; set; }

    public int? PlanId { get; set; }

    public string CompanyName { get; set; }

    public string GUINumber { get; set; }

    public string Title { get; set; }

    public decimal? Price { get; set; }

    public DateTime OrderDate { get; set; }

    public bool Status { get; set; }

    public virtual Company Company { get; set; }

    public virtual PricingPlan Plan { get; set; }
}