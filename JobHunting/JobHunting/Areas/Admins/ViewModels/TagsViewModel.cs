using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class TagsViewModel
    {
        [Key]
        public int TagId { get; set; }

        public int? TagClassId { get; set; }

        public string TagName { get; set; }
    }
}
