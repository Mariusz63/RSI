using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaServer.Models
{
    public class Rezerwacja
    {
        public string Id { get; set; }
        public string SalaId { get; set; }
        public List<int> NumeryMiejsc { get; set; }
        public string ImieNazwisko { get; set; }
        public DateTime DataRezerwacji { get; set; }
        public byte[] PotwierdzeniePdf { get; set; } // MTOM

        // Dane o filmie
        public string FilmId { get; set; }
        public string SeansId { get; set; }
        public string TytulFilmu { get; set; }
        public string RezyserFilmu { get; set; }
        public string OpisFilmu { get; set; }
        public List<string> AktorzyFilmu { get; set; }
        public byte[] ZdjecieFilmu { get; set; }
    }

}
