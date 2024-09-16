using System.ComponentModel.DataAnnotations;

namespace JobHunting.ViewModel
{

    public class LoginViewModel
    {

        public string Role { get; set; }  // 用來區分求職者和徵才者

        [Required(ErrorMessage = "請輸入身分證字號")]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "密碼至少要6位字元")]
        public string Password { get; set; }

        [Required(ErrorMessage = "請輸入統一編號")]
        [MinLength(8, ErrorMessage = "統一編號須為8碼")]
        public int GUINumber { get; set; }
        //[Required(ErrorMessage = "請再次確認密碼")]
        //[Compare("Password", ErrorMessage = "密碼和確認密碼不匹配")]
        //public string ConfirmPassword { get; set; }
    }
}
