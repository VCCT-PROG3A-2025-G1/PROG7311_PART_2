using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_PART_2.Models
{

    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }

        [Required] 
        [ForeignKey("Farmer")]
        public int FarmerId { get; set; } 

        public Farmer? Farmer { get; set; }
    }
}