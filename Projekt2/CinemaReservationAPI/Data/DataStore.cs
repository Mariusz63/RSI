using CinemaReservationAPI.Models;

namespace CinemaReservationAPI.Data
{
    public static class DataStore
    {
        public static List<Movie> Movies { get; } = SampleData.GetMovies();
        public static List<Reservation> Reservations { get; } = new List<Reservation>();
    }

}
