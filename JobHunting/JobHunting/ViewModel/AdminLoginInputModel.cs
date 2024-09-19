using System.ComponentModel.DataAnnotations;

namespace JobHunting.ViewModel
{
    public class AdminLoginInputModel
    {
        [Required(ErrorMessage = "請輸入工號")]
        //[MinLength(4, ErrorMessage = "工號為4碼")]
        //[MaxLength(4, ErrorMessage = "工號為4碼")]
        public int PersonnelCode { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        //[MinLength(6, ErrorMessage = "密碼至少要6位字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
