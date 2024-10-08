namespace Backend.DTO.Venue
{
    public class PutVenueDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public float? Rating { get; set; }
        public string? Category { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
    }
}
