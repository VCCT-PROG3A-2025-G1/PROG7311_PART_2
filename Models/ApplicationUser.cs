using Microsoft.AspNetCore.Identity;

namespace PROG7311_PART_2.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }

      
        public int? FarmerId { get; set; }

        public string Name { get; set; }

    }
}