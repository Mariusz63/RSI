using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CinemaServer.Models;
using CinemaServer.Utils;
using CinemaServer.Interface;
using System.ServiceModel;

namespace CinemaServer.Service
{
    public class CinemaReservationService : ICinemaReservationService
    {
        private static List<Film> filmy = new List<Film>();
        private static List<Rezerwacja> rezerwacje = new List<Rezerwacja>();
        private static List<Uzytkownik> uzytkownicy = new List<Uzytkownik>();
        private static Random rnd = new Random();

        #region Dodanie Filmów
        static CinemaReservationService()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\mariu\\Desktop\\RSI\\Projekt1\\CinemaServer\\Resources\\", "Incepcja.jpg");
            var film1 = new Film
            {
                Id = Guid.NewGuid().ToString(),
                Tytul = "Incepcja",
                Rezyser = "Christopher Nolan",
                Aktorzy = new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
                Opis = "Thriller science fiction",
                Zdjecie = File.Exists(path) ? File.ReadAllBytes(path) : new byte[0],

                Seanse = new List<Seans>
    {
        new Seans
        {
            Id = Guid.NewGuid().ToString(),
            Data = DateTime.Today,
            Godzina = 18,
            Sala = rnd.Next(1,6),
            Miejsca = Enumerable.Range(1, 10).Select(n => new Miejsce { Numer = n, Zajete = false }).ToList()
        }
    }
            };
        
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\mariu\\Desktop\\RSI\\Projekt1\\CinemaServer\\Resources\\", "Matrix.jpg");
            var film2 = new Film
            {
                Id = Guid.NewGuid().ToString(),
                Tytul = "Matrix",
                Rezyser = "Lana Wachowski, Lilly Wachowski",
                Aktorzy = new List<string> { "Keanu Reeves", "Laurence Fishburne" },
                Opis = "Kultowy film science fiction",
                Zdjecie = File.Exists(path) ? File.ReadAllBytes(path) : new byte[0],

                Seanse = new List<Seans>
    {
        new Seans
        {
            Id = Guid.NewGuid().ToString(),
            Data = DateTime.Today.AddDays(1),
            Godzina = 20,
            Sala = rnd.Next(1,6),
            Miejsca = Enumerable.Range(1, 10).Select(n => new Miejsce { Numer = n, Zajete = false }).ToList()
        }
    }
            };

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\mariu\\Desktop\\RSI\\Projekt1\\CinemaServer\\Resources\\", "Interstellar.jpg");
            var film3 = new Film
            {
                Id = Guid.NewGuid().ToString(),
                Tytul = "Interstellar",
                Rezyser = "Christopher Nolan",
                Aktorzy = new List<string> { "Matthew McConaughey", "Anne Hathaway" },
                Opis = "Eksploracja kosmosu i czasoprzestrzeni",
                Zdjecie = File.Exists(path) ? File.ReadAllBytes(path) : new byte[0],

                Seanse = new List<Seans>
    {
        new Seans
        {
            Id = Guid.NewGuid().ToString(),
            Data = DateTime.Today.AddDays(2),
            Godzina = 19,
            Sala = rnd.Next(1,6),
            Miejsca = Enumerable.Range(1, 10).Select(n => new Miejsce { Numer = n, Zajete = false }).ToList()
        }
    }
            };

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\mariu\\Desktop\\RSI\\Projekt1\\CinemaServer\\Resources\\", "Pulp Fiction.jpg");
            var film4 = new Film
            {
                Id = Guid.NewGuid().ToString(),
                Tytul = "Pulp Fiction",
                Rezyser = "Quentin Tarantino",
                Aktorzy = new List<string> { "John Travolta", "Samuel L. Jackson" },
                Opis = "Kultowy film gangsterski",
                Zdjecie = File.Exists(path) ? File.ReadAllBytes(path) : new byte[0],

                Seanse = new List<Seans>
    {
        new Seans
        {
            Id = Guid.NewGuid().ToString(),
            Data = DateTime.Today.AddDays(3),
            Godzina = 21,
            Sala = rnd.Next(1,6),
            Miejsca = Enumerable.Range(1, 10).Select(n => new Miejsce { Numer = n, Zajete = false }).ToList()
        }
    }
            };

            filmy.Add(film1);
            filmy.Add(film2);
            filmy.Add(film3);
            filmy.Add(film4);
            #endregion

            var klient = new Uzytkownik
            {
                Id = Guid.NewGuid().ToString(),
                ImieNazwisko = "Jan Kowalski"
            };

            uzytkownicy.Add(klient);
        }

private void ThrowIfNull(object obj, string message)
{
    if (obj == null)
    {
        Console.WriteLine("ThrowIfNull triggered: " + message);
        throw new FaultException(message);
    }
}


        public string Zaloguj(string imieNazwisko)
        {
            var istnieje = uzytkownicy.FirstOrDefault(u => u.ImieNazwisko == imieNazwisko);
            if (istnieje != null)
                return istnieje.Id;

            var nowy = new Uzytkownik { Id = Guid.NewGuid().ToString(), ImieNazwisko = imieNazwisko };
            uzytkownicy.Add(nowy);
            return nowy.Id;
        }

        public List<Film> GetListaFilmow()
        {
            return filmy;
        }

        public string ZarezerwujMiejsce(string userId, string filmId, string seansId, int numerMiejsca)
        {
            var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Id == userId);
            ThrowIfNull(uzytkownik, "Użytkownik niezalogowany.");

            var film = filmy.FirstOrDefault(f => f.Id == filmId);
            ThrowIfNull(film, "Film nie istnieje.");

            var seans = film.Seanse.FirstOrDefault(s => s.Id == seansId);
            ThrowIfNull(seans, "Seans nie istnieje.");

            var miejsce = seans.Miejsca.FirstOrDefault(m => m.Numer == numerMiejsca);
            if (miejsce == null || miejsce.Zajete)
                throw new FaultException("Miejsce jest już zajęte lub nie istnieje.");

            miejsce.Zajete = true;

            var rezerwacja = new Rezerwacja
            {
                Id = Guid.NewGuid().ToString(),
                UserId = uzytkownik.Id,
                FilmId = filmId,
                SeansId = seansId,
                SalaId = seansId,
                NumeryMiejsc = new List<int> { numerMiejsca },
                ImieNazwisko = uzytkownik.ImieNazwisko,
                DataRezerwacji = DateTime.Now,
                TytulFilmu = film.Tytul,
                RezyserFilmu = film.Rezyser,
                OpisFilmu = film.Opis,
                AktorzyFilmu = film.Aktorzy,
                ZdjecieFilmu = film.Zdjecie
            };

            rezerwacja.PotwierdzeniePdf = PdfGenerator.GenerujPotwierdzenie(rezerwacja);
            rezerwacje.Add(rezerwacja);

            return $"Rezerwacja potwierdzona, ID: {rezerwacja.Id}";
        }

        public string ZarezerwujWieleMiejsc(string userId, string filmId, string seansId, string miejsca)
        {
            try
            {
                ConsoleLogger.Log($"Rozpoczynanie rezerwacji grupowej przez użytkownika {userId} dla filmu {filmId} na seansie {seansId}.", ConsoleLogger.LogLevel.Info);

                var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Id == userId);
                if (uzytkownik == null)
                {
                    ConsoleLogger.Log("Użytkownik niezalogowany.", ConsoleLogger.LogLevel.Error);
                    throw new Exception("Użytkownik niezalogowany.");
                }

                var film = filmy.FirstOrDefault(f => f.Id == filmId);
                if (film == null)
                {
                    ConsoleLogger.Log("Film nie istnieje.", ConsoleLogger.LogLevel.Error);
                    throw new Exception("Film nie istnieje.");
                }

                var seans = film.Seanse.FirstOrDefault(s => s.Id == seansId);
                if (seans == null)
                {
                    ConsoleLogger.Log("Seans nie istnieje.", ConsoleLogger.LogLevel.Error);
                    throw new Exception("Seans nie istnieje.");
                }

                ConsoleLogger.Log($"Znaleziono seans: {seans.Data} o godzinie {seans.Godzina}.", ConsoleLogger.LogLevel.Info);

                var miejscaLista = miejsca.Split(',').Select(int.Parse).ToList(); // Przekształcanie ciągu na listę intów

                if (miejscaLista == null || miejscaLista.Count == 0)
                {
                    throw new FaultException("Lista miejsc jest pusta.");
                }

                foreach (var m in miejscaLista)
                {
                    var miejsce = seans.Miejsca.FirstOrDefault(x => x.Numer == m);
                    if (miejsce == null || miejsce.Zajete)
                    {
                        ConsoleLogger.Log($"Miejsce {m} jest już zajęte lub nie istnieje.", ConsoleLogger.LogLevel.Warning);
                        throw new Exception($"Miejsce {m} jest już zajęte lub nie istnieje.");
                    }
                }

                foreach (var m in miejscaLista)
                {
                    seans.Miejsca.First(x => x.Numer == m).Zajete = true;
                    ConsoleLogger.Log($"Zarezerwowano miejsce numer {m}.", ConsoleLogger.LogLevel.Info);
                }

                var rezerwacja = new Rezerwacja
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = uzytkownik.Id,
                    FilmId = filmId,
                    SeansId = seansId,
                    SalaId = seansId,
                    NumeryMiejsc = miejscaLista,
                    ImieNazwisko = uzytkownik.ImieNazwisko,
                    DataRezerwacji = DateTime.Now,
                    TytulFilmu = film.Tytul,
                    RezyserFilmu = film.Rezyser,
                    OpisFilmu = film.Opis,
                    AktorzyFilmu = film.Aktorzy,
                    ZdjecieFilmu = film.Zdjecie
                };

                rezerwacja.PotwierdzeniePdf = PdfGenerator.GenerujPotwierdzenie(rezerwacja);
                rezerwacje.Add(rezerwacja);

                ConsoleLogger.Log($"Rezerwacja zakończona. ID rezerwacji: {rezerwacja.Id}", ConsoleLogger.LogLevel.Info);
                return $"Zarezerwowano miejsca: {string.Join(", ", miejscaLista)}. ID: {rezerwacja.Id}";
            }
            catch (Exception ex)
            {
                ConsoleLogger.Log($"Błąd: {ex.Message}", ConsoleLogger.LogLevel.Error);
                return $"Wystąpił błąd: {ex.Message}";
            }
        }

        public bool AnulujRezerwacje(string userId, string rezerwacjaId)
        {
            var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Id == userId);
            ThrowIfNull(uzytkownik, "Użytkownik niezalogowany.");

            var rezerwacja = rezerwacje.FirstOrDefault(r => r.Id == rezerwacjaId && r.UserId == userId);
            ThrowIfNull(rezerwacja, "Rezerwacja nie została znaleziona.");

            var film = filmy.FirstOrDefault(f => f.Id == rezerwacja.FilmId);
            var seans = film?.Seanse.FirstOrDefault(s => s.Id == rezerwacja.SeansId);

            if (seans != null)
            {
                foreach (var nr in rezerwacja.NumeryMiejsc)
                {
                    var miejsce = seans.Miejsca.FirstOrDefault(m => m.Numer == nr);
                    if (miejsce != null)
                        miejsce.Zajete = false;
                }
            }

            rezerwacje.Remove(rezerwacja);
            return true;
        }

        public bool ModyfikujRezerwacje(string userId, string rezerwacjaId, int noweMiejsce)
        {
            var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Id == userId);
            ThrowIfNull(uzytkownik, "Użytkownik niezalogowany.");

            var rezerwacja = rezerwacje.FirstOrDefault(r => r.Id == rezerwacjaId && r.UserId == userId);
            ThrowIfNull(rezerwacja, "Rezerwacja nie została znaleziona.");

            if (rezerwacja.NumeryMiejsc.Count != 1)
                throw new FaultException("Modyfikacja dotyczy tylko pojedynczych rezerwacji.");

            var film = filmy.FirstOrDefault(f => f.Id == rezerwacja.FilmId);
            var seans = film?.Seanse.FirstOrDefault(s => s.Id == rezerwacja.SeansId);
            ThrowIfNull(seans, "Seans nie istnieje.");

            var nowe = seans.Miejsca.FirstOrDefault(m => m.Numer == noweMiejsce);
            if (nowe == null || nowe.Zajete)
                throw new FaultException("Nowe miejsce jest zajęte lub nie istnieje.");

            var stareMiejsce = rezerwacja.NumeryMiejsc[0];
            seans.Miejsca.First(x => x.Numer == stareMiejsce).Zajete = false;
            nowe.Zajete = true;

            rezerwacja.NumeryMiejsc[0] = noweMiejsce;
            rezerwacja.PotwierdzeniePdf = PdfGenerator.GenerujPotwierdzenie(rezerwacja);
            return true;
        }

        public Rezerwacja PobierzPotwierdzenieRezerwacji(string userId, string rezerwacjaId)
        {
            var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Id == userId);
            ThrowIfNull(uzytkownik, "Użytkownik niezalogowany.");

            var rezerwacja = rezerwacje.FirstOrDefault(r => r.Id == rezerwacjaId && r.UserId == userId);
            ThrowIfNull(rezerwacja, "Rezerwacja nie została znaleziona.");

            return rezerwacja;
        }

        public List<Rezerwacja> ListaRezerwacji(string userId)
        {
            var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Id == userId);
            ThrowIfNull(uzytkownik, "Użytkownik niezalogowany.");

            return rezerwacje.Where(r => r.ImieNazwisko == uzytkownik.ImieNazwisko).ToList();
        }

        public Film GetFilmInfo(string filmId)
        {
            var film = filmy.FirstOrDefault(f => f.Id == filmId);
            if (film == null)
                throw new FaultException("Film nie został znaleziony.");

            return film;
        }

        public Seans GetSeansInfo(string seansId)
        {
            foreach (var film in filmy)
            {
                var seans = film.Seanse.FirstOrDefault(s => s.Id == seansId);
                if (seans != null)
                    return seans;
            }

            throw new FaultException("Seans nie został znaleziony.");
        }


    }
}