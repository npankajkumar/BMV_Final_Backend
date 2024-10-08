namespace Backend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Venue> Venues { get; set; }
    }
}
