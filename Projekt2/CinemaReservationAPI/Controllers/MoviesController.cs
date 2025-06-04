using CinemaReservationAPI.Data;
using CinemaReservationAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaReservationAPI.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController : ControllerBase
    {
        private static List<Movie> _movies = SampleData.GetMovies();

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var result = _movies.Select(m => new {
                m.Id,
                m.Title,
                Showtimes = m.Showtimes.Select(s => new {
                    s.Id,
                    Day = s.Time.ToString("yyyy-MM-dd"),
                    Time = s.Time.ToString("HH:mm")
                })
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetMovieDetailsById(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();

            var result = new
            {
                movie.Id,
                movie.Title,
                movie.Director,
                movie.Actors,
                movie.Description,
                Showtimes = movie.Showtimes.Select(s => new {
                    s.Id,
                    Day = s.Time.ToString("yyyy-MM-dd"),
                    Time = s.Time.ToString("HH:mm"),
                    Seats = s.Seats.Select(seat => new {
                        seat.Number,
                        seat.IsReserved
                    })
                })
            };

            return Ok(result);
        }

    }
}
