using CinemaReservationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaReservationAPI.Data
{
    public static class SampleData
    {
        public static List<Movie> GetMovies()
        {
            return new List<Movie> {
                new Movie {
                    Id = 1,
                    Title = "Inception",
                    Director = "Christopher Nolan",
                    Actors = new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
                    Description = "Mind-bending sci-fi thriller",
                    Image = new byte[0], // Leave empty for now
                    Showtimes = new List<Showtime> {
                        new Showtime {
                            Id = 1,
                            Time = DateTime.Today.AddHours(18),
                            Seats = Enumerable.Range(1, 20).Select(n => new Seat { Number = n, IsReserved = false }).ToList()
                        }
                    }
                },
                new Movie {
                    Id = 2,
                    Title = "The Matrix",
                    Director = "The Wachowskis",
                    Actors = new List<string> { "Keanu Reeves", "Carrie-Anne Moss" },
                    Description = "Cyberpunk classic",
                    Image = new byte[0],
                    Showtimes = new List<Showtime> {
                        new Showtime {
                            Id = 2,
                            Time = DateTime.Today.AddHours(20),
                            Seats = Enumerable.Range(1, 25).Select(n => new Seat { Number = n, IsReserved = false }).ToList()
                        }
                    }
                },
                new Movie {
                    Id = 3,
                    Title = "Interstellar",
                    Director = "Christopher Nolan",
                    Actors = new List<string> { "Matthew McConaughey", "Anne Hathaway" },
                    Description = "Epic science fiction adventure",
                    Image = new byte[0],
                    Showtimes = new List<Showtime> {
                        new Showtime {
                            Id = 3,
                            Time = DateTime.Today.AddHours(19),
                            Seats = Enumerable.Range(1, 30).Select(n => new Seat { Number = n, IsReserved = false }).ToList()
                        }
                    }
                }
            };
        }
    }
}
