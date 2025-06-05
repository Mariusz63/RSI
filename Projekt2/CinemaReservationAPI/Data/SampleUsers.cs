using CinemaReservationAPI.Models;

namespace CinemaReservationAPI.Data
{
    public class SampleUsers
    {
        public static List<User> Users { get; } = new List<User>
        {
            new User { Id = 1, Name = "Jan", SecondName = "Kowalski", Password = "haslo123" },
            new User { Id = 2, Name = "Anna", SecondName = "Nowak", Password = "tajnehaslo" },
            new User { Id = 3, Name = "Piotr", SecondName = "Zielinski", Password = "qwerty" }
        };
    }
}
