using CinemaReservationAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace CinemaReservationAPI.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            Console.WriteLine("GET /movies");
            var result = DataStore.Movies.Select(m => new {
                m.Id,
                m.Title,
                Showtimes = m.Showtimes.Select(s => new {
                    s.Id,
                    Day = s.Time.ToString("yyyy-MM-dd"),
                    Time = s.Time.ToString("HH:mm"),
                    _links = new
                    {
                        self = Url.Action(nameof(GetShowtimeSeats), new { movieId = m.Id, showtimeId = s.Id }),
                        reserve = Url.Action("CreateReservation", "Reservations")
                    }
                }),
                _links = new
                {
                    self = Url.Action(nameof(GetMovieDetailsById), new { id = m.Id }),
                    image = Url.Action(nameof(GetMovieImage), new { id = m.Id })
                }
            });

            return Ok(new
            {
                Movies = result,
                _links = new
                {
                    self = Url.Action(nameof(GetAllMovies))
                }
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetMovieDetailsById(int id)
        {
            Console.WriteLine($"GET /movies/{id}");
            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();

            var result = new
            {
                movie.Id,
                movie.Title,
                movie.Director,
                movie.Actors,
                movie.Description,
                ImageBase64 = movie.Image != null ? Convert.ToBase64String(movie.Image) : null,
                Showtimes = movie.Showtimes.Select(s => new {
                    s.Id,
                    Day = s.Time.ToString("yyyy-MM-dd"),
                    Time = s.Time.ToString("HH:mm"),
                    Seats = s.Seats.Select(seat => new {
                        seat.Number,
                        seat.IsReserved
                    }),
                    _links = new
                    {
                        self = Url.Action(nameof(GetShowtimeSeats), new { movieId = movie.Id, showtimeId = s.Id }),
                        reserve = Url.Action("CreateReservation", "Reservations")
                    }
                }),
                _links = new
                {
                    self = Url.Action(nameof(GetMovieDetailsById), new { id = movie.Id }),
                    image = Url.Action(nameof(GetMovieImage), new { id = movie.Id }),
                    all_movies = Url.Action(nameof(GetAllMovies))
                }
            };

            return Ok(result);
        }

        [HttpGet("{id}/image")]
        public IActionResult GetMovieImage(int id)
        {
            Console.WriteLine($"GET /movies/{id}/image");
            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null || movie.Image == null || movie.Image.Length == 0)
                return NotFound("Brak obrazu.");

            return File(movie.Image, "image/jpeg");
        }

        [HttpGet("{movieId}/showtimes/{showtimeId}/seats")]
        public IActionResult GetShowtimeSeats(int movieId, int showtimeId)
        {
            Console.WriteLine($"GET /movies/{movieId}/showtimes/{showtimeId}/seats");
            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == movieId);
            var showtime = movie?.Showtimes.FirstOrDefault(s => s.Id == showtimeId);

            if (showtime == null)
                return NotFound();

            var result = showtime.Seats.Select(seat => new {
                seat.Number,
                seat.IsReserved
            });

            return Ok(new
            {
                MovieId = movieId,
                ShowtimeId = showtimeId,
                Seats = result,
                _links = new
                {
                    self = Url.Action(nameof(GetShowtimeSeats), new { movieId, showtimeId }),
                    movie = Url.Action(nameof(GetMovieDetailsById), new { id = movieId })
                }
            });
        }
    }
}
