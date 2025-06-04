namespace CinemaReservationAPI.Models
{
    public class Showtime
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public List<Seat> Seats { get; set; }
    }

}
