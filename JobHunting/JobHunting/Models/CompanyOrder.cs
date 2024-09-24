﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class CompanyOrder
{
    public string OrderId { get; set; }

    public int? CompanyId { get; set; }

    public int? PlanId { get; set; }

    public int OrderNumber { get; set; }

    public string CompanyName { get; set; }

    public string GUINumber { get; set; }

    public string Title { get; set; }

    public decimal Price { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? PayDate { get; set; }

    public int Duration { get; set; }

    public bool Status { get; set; }

    public string StatusType { get; set; }

    public virtual Company Company { get; set; }

    public virtual PricingPlan Plan { get; set; }
}