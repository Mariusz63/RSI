namespace CinemaReservationAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public List<string> Actors { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; } // MTOM or base64
        public List<Showtime> Showtimes { get; set; }
    }
}
