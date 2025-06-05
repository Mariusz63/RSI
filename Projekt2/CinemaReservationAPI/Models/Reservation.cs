using System.Text.Json.Serialization;

namespace CinemaReservationAPI.Models
{
    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int MovieId { get; set; }
        public int ShowtimeId { get; set; }
        public List<int> SeatNumbers { get; set; }
        [JsonIgnore]
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
