namespace JobHunting.Areas.Admins.ViewModels
{
    public class OpinioLetterOutputViewModel
    {
        public int LetterId { get; set; }
        public string Class { get; set; }

        public string SubjectLine { get; set; }

        public string Content { get; set; }
        public byte[] Attachment { get; set; }

        public bool Status { get; set; }
    }
}
