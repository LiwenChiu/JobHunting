﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Candidates.Models;

public partial class PricingPlan
{
    public int PlanID { get; set; }

    public string Title { get; set; }

    public string Intro { get; set; }

    public int Duration { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<CompanyOrder> CompanyOrders { get; set; } = new List<CompanyOrder>();
}