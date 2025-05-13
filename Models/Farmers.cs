namespace PROG7311_PART_2.Models
{
    public class Farmer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public List<Product> Products { get; set; }
    }
}