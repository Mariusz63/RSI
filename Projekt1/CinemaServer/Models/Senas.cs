using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaServer.Models
{

    public class Seans
    {
        public string Id { get; set; }
        public DateTime Data { get; set; }
        public int Godzina { get; set; }
        public int Sala { get; set; }
        public List<Miejsce> Miejsca { get; set; }
    }

}
