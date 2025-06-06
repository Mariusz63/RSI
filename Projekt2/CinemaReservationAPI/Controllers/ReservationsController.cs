using CinemaReservationAPI.Data;
using CinemaReservationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace CinemaReservationAPI.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationsController : ControllerBase
    {
        private string GetCurrentUserName()
        {
            return HttpContext.Items["User"]?.ToString() ?? "anonymous";
        }

        [HttpGet]
        public IActionResult GetUserReservations()
        {
            Console.WriteLine("GET /reservations");

            var username = GetCurrentUserName();
            var userReservations = DataStore.Reservations
                .Where(r => r.UserName == username)
                .Select(r => new
                {
                    r.Id,
                    r.MovieId,
                    r.ShowtimeId,
                    r.SeatNumbers,
                    r.CreatedAt,
                    _links = new
                    {
                        self = Url.Action(nameof(GetReservation), new { id = r.Id }),
                        details = Url.Action(nameof(GetReservationDetails), new { id = r.Id }),
                        pdf = Url.Action(nameof(GetReservationPdf), new { id = r.Id }),
                        cancel = Url.Action(nameof(DeleteReservation), new { id = r.Id })
                    }
                })
                .ToList();

            return Ok(new
            {
                Reservations = userReservations,
                _links = new
                {
                    self = Url.Action(nameof(GetUserReservations)),
                    create = Url.Action(nameof(CreateReservation))
                }
            });
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation request)
        {
            Console.WriteLine("POST /reservations");
            Console.WriteLine($"Request body: {System.Text.Json.JsonSerializer.Serialize(request)}");

            if (!string.IsNullOrWhiteSpace(request.UserName))
                return BadRequest("Pole 'UserName' nie powinno być ustawiane przez klienta.");

            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == request.MovieId);
            if (movie == null)
                return NotFound("Film nie znaleziony.");

            var showtime = movie.Showtimes.FirstOrDefault(s => s.Id == request.ShowtimeId);
            if (showtime == null)
                return NotFound("Seans nie znaleziony.");

            if (request.SeatNumbers == null || !request.SeatNumbers.Any())
                return BadRequest("Musisz wybrać przynajmniej jedno miejsce.");

            foreach (var seatNumber in request.SeatNumbers)
            {
                var seat = showtime.Seats.FirstOrDefault(s => s.Number == seatNumber);
                if (seat == null)
                    return BadRequest($"Miejsce {seatNumber} nie istnieje.");
                if (seat.IsReserved)
                    return Conflict($"Miejsce {seatNumber} jest już zarezerwowane.");
            }

            foreach (var seatNumber in request.SeatNumbers)
                showtime.Seats.First(s => s.Number == seatNumber).IsReserved = true;

            request.Id = Guid.NewGuid();
            request.UserName = GetCurrentUserName();
            request.CreatedAt = DateTime.UtcNow;
            DataStore.Reservations.Add(request);

            return CreatedAtAction(nameof(GetReservation), new { id = request.Id }, new
            {
                request.Id,
                Status = "confirmed",
                _links = new
                {
                    self = Url.Action(nameof(GetReservation), new { id = request.Id }),
                    details = Url.Action(nameof(GetReservationDetails), new { id = request.Id }),
                    pdf = Url.Action(nameof(GetReservationPdf), new { id = request.Id }),
                    cancel = Url.Action(nameof(DeleteReservation), new { id = request.Id })
                }
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetReservation(Guid id)
        {
            Console.WriteLine($"GET /reservations/{id}");

            var username = GetCurrentUserName();
            var res = DataStore.Reservations.FirstOrDefault(r => r.Id == id && r.UserName == username);
            if (res == null) return NotFound();

            return Ok(new
            {
                res.Id,
                res.MovieId,
                res.ShowtimeId,
                res.SeatNumbers,
                res.CreatedAt,
                _links = new
                {
                    self = Url.Action(nameof(GetReservation), new { id = res.Id }),
                    details = Url.Action(nameof(GetReservationDetails), new { id = res.Id }),
                    pdf = Url.Action(nameof(GetReservationPdf), new { id = res.Id }),
                    update = Url.Action(nameof(UpdateReservation), new { id = res.Id }),
                    cancel = Url.Action(nameof(DeleteReservation), new { id = res.Id })
                }
            });
        }

        [HttpGet("{id}/Details")]
        public IActionResult GetReservationDetails(Guid id)
        {
            Console.WriteLine($"GET /reservations/{id}/Details");

            var username = GetCurrentUserName();
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id && r.UserName == username);
            if (reservation == null) return NotFound();

            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == reservation.MovieId);

            return Ok(new
            {
                reservation.Id,
                reservation.UserName,
                reservation.ShowtimeId,
                reservation.SeatNumbers,
                reservation.CreatedAt,
                Movie = movie == null ? null : new
                {
                    movie.Id,
                    movie.Title,
                    movie.Director,
                    Actors = movie.Actors,
                    movie.Description,
                    ImageBase64 = movie.Image != null ? Convert.ToBase64String(movie.Image) : null
                },
                _links = new
                {
                    self = Url.Action(nameof(GetReservationDetails), new { id = reservation.Id }),
                    reservation = Url.Action(nameof(GetReservation), new { id = reservation.Id }),
                    pdf = Url.Action(nameof(GetReservationPdf), new { id = reservation.Id }),
                    update = Url.Action(nameof(UpdateReservation), new { id = reservation.Id }),
                    cancel = Url.Action(nameof(DeleteReservation), new { id = reservation.Id })
                }
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(Guid id)
        {
            Console.WriteLine($"DELETE /reservations/{id}");

            var username = GetCurrentUserName();
            var res = DataStore.Reservations.FirstOrDefault(r => r.Id == id && r.UserName == username);
            if (res == null) return NotFound();

            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == res.MovieId);
            var showtime = movie?.Showtimes.FirstOrDefault(s => s.Id == res.ShowtimeId);

            if (showtime != null)
            {
                foreach (var seatNumber in res.SeatNumbers)
                {
                    var seat = showtime.Seats.FirstOrDefault(s => s.Number == seatNumber);
                    if (seat != null)
                        seat.IsReserved = false;
                }
            }

            DataStore.Reservations.Remove(res);

            return Ok(new
            {
                Status = "deleted",
                _links = new
                {
                    self = Url.Action(nameof(GetUserReservations)),
                    create = Url.Action(nameof(CreateReservation))
                }
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(Guid id, [FromBody] List<int> newSeatNumbers)
        {
            Console.WriteLine($"PUT /reservations/{id}");
            Console.WriteLine($"Request body: {System.Text.Json.JsonSerializer.Serialize(newSeatNumbers)}");

            var username = GetCurrentUserName();
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id && r.UserName == username);
            if (reservation == null)
                return NotFound("Rezerwacja nie znaleziona lub nie należy do Ciebie.");

            if (newSeatNumbers == null || !newSeatNumbers.Any())
                return BadRequest("Musisz wybrać przynajmniej jedno miejsce.");

            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == reservation.MovieId);
            var showtime = movie?.Showtimes.FirstOrDefault(s => s.Id == reservation.ShowtimeId);
            if (showtime == null)
                return NotFound("Seans nie znaleziony.");

            foreach (var oldSeatNumber in reservation.SeatNumbers)
            {
                var seat = showtime.Seats.FirstOrDefault(s => s.Number == oldSeatNumber);
                if (seat != null) seat.IsReserved = false;
            }

            foreach (var newSeatNumber in newSeatNumbers)
            {
                var seat = showtime.Seats.FirstOrDefault(s => s.Number == newSeatNumber);
                if (seat == null)
                    return BadRequest($"Miejsce {newSeatNumber} nie istnieje.");
                if (seat.IsReserved)
                    return Conflict($"Miejsce {newSeatNumber} jest już zarezerwowane.");
            }

            foreach (var newSeatNumber in newSeatNumbers)
                showtime.Seats.First(s => s.Number == newSeatNumber).IsReserved = true;

            reservation.SeatNumbers = newSeatNumbers;

            return Ok(new
            {
                reservation.Id,
                UpdatedSeats = reservation.SeatNumbers,
                Status = "updated",
                _links = new
                {
                    self = Url.Action(nameof(GetReservation), new { id = reservation.Id }),
                    details = Url.Action(nameof(GetReservationDetails), new { id = reservation.Id }),
                    pdf = Url.Action(nameof(GetReservationPdf), new { id = reservation.Id }),
                    cancel = Url.Action(nameof(DeleteReservation), new { id = reservation.Id })
                }
            });
        }

        [HttpGet("{id}/pdf")]
        public IActionResult GetReservationPdf(Guid id)
        {
            Console.WriteLine($"GET /reservations/{id}/pdf");

            var username = GetCurrentUserName();
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id && r.UserName == username);
            if (reservation == null) return NotFound();

            var movie = DataStore.Movies.FirstOrDefault(m => m.Id == reservation.MovieId);

            using (var stream = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 12, XFontStyleEx.Regular);
                int y = 40;

                gfx.DrawString($"Reservation ID: {reservation.Id}", font, XBrushes.Black, new XPoint(40, y)); y += 25;
                gfx.DrawString($"Movie: {movie?.Title ?? "Nieznany"}", font, XBrushes.Black, new XPoint(40, y)); y += 25;
                if (movie != null)
                {
                    gfx.DrawString($"Director: {movie.Director}", font, XBrushes.Black, new XPoint(40, y)); y += 20;
                    gfx.DrawString("Actors:", font, XBrushes.Black, new XPoint(40, y)); y += 20;
                    foreach (var actor in movie.Actors)
                    {
                        gfx.DrawString($"- {actor}", font, XBrushes.Black, new XPoint(60, y)); y += 20;
                    }
                    gfx.DrawString("Description:", font, XBrushes.Black, new XPoint(40, y)); y += 20;
                    var desc = movie.Description ?? "";
                    var maxWidth = page.Width - 80;
                    var descLines = SplitTextIntoLines(desc, gfx, font, maxWidth);
                    foreach (var line in descLines)
                    {
                        gfx.DrawString(line, font, XBrushes.Black, new XPoint(40, y)); y += 20;
                    }
                    y += 10;

                    if (movie.Image != null && movie.Image.Length > 0)
                    {
                        try
                        {
                            using (var ms = new MemoryStream(movie.Image))
                            {
                                var image = XImage.FromStream(ms);
                                gfx.DrawImage(image, 40, y, 200, 300);
                            }
                        }
                        catch
                        {
                            gfx.DrawString("Błąd ładowania obrazka.", font, XBrushes.Red, new XPoint(40, y));
                        }
                    }
                }
                y += 320;

                gfx.DrawString($"Showtime ID: {reservation.ShowtimeId}", font, XBrushes.Black, new XPoint(40, y)); y += 25;
                gfx.DrawString($"User: {reservation.UserName}", font, XBrushes.Black, new XPoint(40, y)); y += 25;
                gfx.DrawString($"Created At: {reservation.CreatedAt}", font, XBrushes.Black, new XPoint(40, y)); y += 25;
                gfx.DrawString($"Seats: {string.Join(", ", reservation.SeatNumbers)}", font, XBrushes.Black, new XPoint(40, y)); y += 25;

                document.Save(stream, false);
                var bytes = stream.ToArray();
                return File(bytes, "application/pdf", $"reservation_{id}.pdf");
            }
        }

        private List<string> SplitTextIntoLines(string text, XGraphics gfx, XFont font, double maxWidth)
        {
            var lines = new List<string>();
            var words = text.Split(' ');
            var currentLine = "";

            foreach (var word in words)
            {
                var testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                var size = gfx.MeasureString(testLine, font);
                if (size.Width > maxWidth)
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        lines.Add(currentLine);
                        currentLine = word;
                    }
                    else
                    {
                        lines.Add(testLine);
                        currentLine = "";
                    }
                }
                else
                {
                    currentLine = testLine;
                }
            }
            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine);

            return lines;
        }
    }
}
