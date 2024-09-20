using System.ComponentModel.DataAnnotations;

namespace JobHunting.ViewModel
{
    public class CompanyRegisterVM
    {
        public string CompanyName { get; set; } 

        [Required(ErrorMessage = "請輸入統一編號")]
        [MinLength(8, ErrorMessage = "統一編號須為8碼")]
        [MaxLength(8, ErrorMessage = "統一編號須為8碼")]
        public string GUINumber { get; set; }

        [Required(ErrorMessage = "請輸入聯絡人姓名")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "請輸入連絡電話")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "請輸入地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "密碼至少要6位字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        public bool Status { get; set; }

        public DateTime Date { get; set; }
    }
}
