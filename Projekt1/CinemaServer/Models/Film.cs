using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CinemaServer.Models
{
    public class Film
    {
        public string Id { get; set; }
        public string Tytul { get; set; }
        public string Rezyser { get; set; }
        public List<string> Aktorzy { get; set; }
        public string Opis { get; set; }
        public byte[] Zdjecie { get; set; } // MTOM
        public List<Seans> Seanse { get; set; }
    }

}
