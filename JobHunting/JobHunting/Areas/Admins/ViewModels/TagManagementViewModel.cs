using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class TagManagementViewModel
    {
        [Display(Name="標籤類型ID")]
        public int? TagClassId { get; set; }

        [Required(ErrorMessage ="標籤類型未填寫")]
        [StringLength(maximumLength: 30, MinimumLength = 1, ErrorMessage = "標籤類型長度不合法")]
        [Display(Name = "標籤類型")]
        public string? TagClassName { get; set; }

        [Key]
        [Display(Name = "標籤ID")]
        public int TagId { get; set; }

        [Required(ErrorMessage = "標籤未填寫")]
        [StringLength(maximumLength: 30, MinimumLength = 1, ErrorMessage = "標籤長度不合法")]
        [Display(Name = "標籤")]
        public string? TagName { get; set; }
    }
}