namespace JobHunting.Areas.Candidates.ViewModels
{
    public class Can_OpinionLetterModalViewModel
    {
        public int LetterId { get; set; }
        public string Class { get; set; }

        public string SubjectLine { get; set; }
        public DateTime SendTime { get; set; }

        public string Content { get; set; }
        public byte[] Attachment { get; set; }

        public bool Status { get; set; }
    }
}
