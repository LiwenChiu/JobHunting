using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class TagsViewModel
    {
        [Key]
        public int TagID { get; set; }

        public int? TagClassID { get; set; }

        public string TagName { get; set; }
    }
}
