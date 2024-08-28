using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobHunting.Data
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(10)]
        public string NationalID { get; set; }
    }
    
}
