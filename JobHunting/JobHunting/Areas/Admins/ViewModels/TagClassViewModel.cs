using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class TagClassViewModel
    {
        [Key]
        public int TagClassId { get; set; }

        public string TagClassName { get; set; }
    }
}
