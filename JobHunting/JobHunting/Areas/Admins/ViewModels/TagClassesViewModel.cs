using System.ComponentModel.DataAnnotations;

namespace JobHunting.Areas.Admins.ViewModels
{
    public class TagClassesViewModel
    {
        [Key]
        public int TagClassId { get; set; }

        public string TagClassName { get; set; }
    }
}
