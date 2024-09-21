using JobHunting.Areas.Candidates.Models;

namespace JobHunting.Areas.Candidates.ViewModels
{
    public class GetCandidateResumesViewModel
    {
        public int ResumeId { get; set; }

        public string Title { get; set; }

        public DateTime LastEditTime { get; set; }
    }
}
