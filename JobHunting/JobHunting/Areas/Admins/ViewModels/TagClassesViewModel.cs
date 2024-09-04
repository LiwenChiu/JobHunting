using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class TagClassesViewModel
    {
        [Key]
        public int TagClassID { get; set; }

        public string TagClassName { get; set; }
    }
}
