using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class CompanyOrder
{
    public int OrderId { get; set; }

    public int? CompanyId { get; set; }

    public int? PlanId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Guinumber { get; set; } = null!;

    public string Title { get; set; } = null!;

    public decimal? Price { get; set; }

    public DateTime OrderDate { get; set; }

    public bool Status { get; set; }

    public virtual Company? Company { get; set; }

    public virtual PricingPlan? Plan { get; set; }
}
