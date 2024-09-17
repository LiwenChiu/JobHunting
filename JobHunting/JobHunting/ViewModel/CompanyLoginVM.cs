using System.ComponentModel.DataAnnotations;

namespace JobHunting.ViewModel
{

    public class CompanyLoginVM
    {

        //public string Role { get; set; }  // 用來區分求職者和徵才者

        [Required(ErrorMessage = "請輸入統一編號")]
        [MinLength(8, ErrorMessage = "統一編號須為8碼")]
        public string GUINumber { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "密碼至少要6位字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       

    }
}
