using CinemaReservationAPI.Data;
using CinemaReservationAPI.Models;
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
            var result = DataStore.Movies.Select(m => new {
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
            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();

            var result = new
            {
                movie.Id,
                movie.Title,
                movie.Director,
                movie.Actors,
                movie.Description,
                ImageBase64 = movie.Image,
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

        [HttpGet("{id}/image")]
        public IActionResult GetMovieImage(int id)
        {
            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null || movie.Image == null || movie.Image.Length == 0)
                return NotFound("Brak obrazu.");

            return File(movie.Image, "image/jpeg");
        }

    }
}
