using System.ComponentModel.DataAnnotations;

namespace JobHunting.ViewModel
{
    public class CandidateRegisterVM
    {
        [Required(ErrorMessage = "身分證字號未填寫")]
        [MaxLength(10)]
        [MinLength(10)]
        [Display(Name = "身分證字號")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "密碼至少要6位字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string? ConfirmPassword { get; set; }

       
    }
}
