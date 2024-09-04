using JobHunting.Areas.Companies.Models;
using JobHunting.Models;
using System.ComponentModel;

namespace JobHunting.Areas.Companies.ViewModel
{
    public class ReceiveResumeViewModel
    {
        public DateOnly? ApplyDate { get; set; }
        public string Title { get; set; }
        public string TitleClassName { get; set; }
    }
}
