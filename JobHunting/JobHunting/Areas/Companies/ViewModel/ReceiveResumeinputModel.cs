using JobHunting.Areas.Companies.ViewModel;
using JobHunting.Models;
using System.ComponentModel;


namespace JobHunting.Areas.Companies.ViewModel
{
    public class ReceiveResumeinputModel
    {
        [DisplayName("應徵日期")]
        public string ApplyDate { get; set; }
        //[DisplayName("職缺名稱")]
        //public string Title { get; set; }
        //[DisplayName("職缺類別")]
        //public string TitleClassName { get; set; }

        public bool? InterviewYN { get; set; }

        public bool? HireYN { get; set; }

        public string Name { get; set; }

        //public bool? Sex { get; set; }


        public int CompanyId { get; set; }

        public string OpeningTitle { get; set; }
        public int CandidateId { get; set; }

        public int ResumeId { get; set; }
        public int OpeningId { get; set; }

    }
}
