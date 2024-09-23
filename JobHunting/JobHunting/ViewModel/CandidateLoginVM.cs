using System.ComponentModel.DataAnnotations;

namespace JobHunting.ViewModel
{
    public class CandidateLoginVM
    {
        public string CandidateLogin { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "密碼至少要6位字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
