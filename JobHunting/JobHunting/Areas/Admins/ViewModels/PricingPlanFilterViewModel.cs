﻿namespace JobHunting.Areas.Admins.ViewModels
{
    public class PricingPlanFilterViewModel
    {
        public int PlanId { get; set; }

        public string Title { get; set; }

        public string Intro { get; set; }

        public int Duration { get; set; }

        public decimal? Price { get; set; }

        public decimal? Discount { get; set; }

        public bool Status { get; set; }
    }
}
