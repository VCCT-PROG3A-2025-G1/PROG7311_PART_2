namespace PROG7311_PART_2.Models
{
    using System.Collections.Generic;

    /// ViewModel for displaying and editing a Product along with a list of available Farmers.
    public class ProductViewModel
    {
        public Product Product { get; set; } = new Product();
        public List<Farmer> Farmers { get; set; } = new List<Farmer>();

        /// Initializes a new instance of the <see cref="ProductViewModel"/> class.
        public ProductViewModel()
        {
            Farmers = new List<Farmer>();
        }

        /// Initializes a new instance of the <see cref="ProductViewModel"/> class with specified product and farmers.
        public ProductViewModel(Product product, IEnumerable<Farmer> farmers)
        {
            Product = product ?? new Product();
            Farmers = new List<Farmer>(farmers ?? new List<Farmer>());
        }
    }
}