﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobHunting.Areas.Candidates.Models;

public partial class CompanyOrder
{
    public int OrderID { get; set; }

    public int? CompanyID { get; set; }

    public int? PlanID { get; set; }

    public string CompanyName { get; set; }

    public int GUINumber { get; set; }

    public string PlanTitle { get; set; }

    public decimal? Price { get; set; }

    public DateTime OrderDate { get; set; }

    public bool Status { get; set; }

    public virtual Company Company { get; set; }

    public virtual PricingPlan Plan { get; set; }
}