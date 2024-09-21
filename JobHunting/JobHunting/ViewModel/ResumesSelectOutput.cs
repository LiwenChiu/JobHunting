namespace JobHunting.ViewModel
{
    public class ResumesSelectOutput
    {
        public int TotalDataCount { get; set; }
        public IEnumerable<ResumesOutput> ResumeOutput { get; set; }
    }
}
