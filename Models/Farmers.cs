using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_PART_2.Models
{

    public class Farmer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; }

        public string? UserId { get; set; }

       
        public ApplicationUser User { get; set; }
    }

}