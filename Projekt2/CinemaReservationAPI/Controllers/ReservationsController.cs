using CinemaReservationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CinemaReservationAPI.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationsController : ControllerBase
    {
        private static List<Reservation> _reservations = new();

        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation request)
        {
            // Dummy logic: No double-book check for brevity
            _reservations.Add(request);

            return CreatedAtAction(nameof(GetReservation), new { id = request.Id }, new
            {
                request.Id,
                Status = "confirmed",
                _links = new
                {
                    pdf = Url.Action(nameof(GetReservationPdf), new { id = request.Id }),
                    cancel = Url.Action(nameof(DeleteReservation), new { id = request.Id })
                }
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetReservation(Guid id)
        {
            var res = _reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(Guid id)
        {
            var res = _reservations.FirstOrDefault(r => r.Id == id);
            if (res == null) return NotFound();

            _reservations.Remove(res);
            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        public IActionResult GetReservationPdf(Guid id)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound();

            // Fake PDF for now
            var bytes = System.Text.Encoding.UTF8.GetBytes($"Reservation ID: {reservation.Id}");
            return File(bytes, "application/pdf", $"reservation_{id}.pdf");
        }
    }

}
