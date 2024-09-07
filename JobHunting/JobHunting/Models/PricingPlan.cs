﻿using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class PricingPlan
{
    public int PlanId { get; set; }

    public string Title { get; set; } = null!;

    public string? Intro { get; set; }

    public int Duration { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<CompanyOrder> CompanyOrders { get; set; } = new List<CompanyOrder>();
}
