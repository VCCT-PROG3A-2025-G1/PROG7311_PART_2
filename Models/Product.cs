namespace PROG7311_PART_2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }
        public int FarmerId { get; set; }
        public Farmer Farmer { get; set; }
    }
}