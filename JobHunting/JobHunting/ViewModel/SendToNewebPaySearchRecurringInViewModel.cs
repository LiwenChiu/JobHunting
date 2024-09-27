namespace JobHunting.ViewModel
{
    public class SendToNewebPaySearchRecurringInViewModel
    {
        public string OrderId { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public int Price { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? PayDate { get; set; }

        public bool Status { get; set; }

        public string StatusType { get; set; }
    }
}
