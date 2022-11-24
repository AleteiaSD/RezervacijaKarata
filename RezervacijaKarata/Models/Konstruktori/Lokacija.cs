using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RezervacijaKarata.Models
{
    public class Lokacija
    {
        public Lokacija(string geografskaDuzina, string geografskaSirina, MestoOdrzavanja mestoOdrzavanja)
        {
            GeografskaDuzina = geografskaDuzina;
            GeografskaSirina = geografskaSirina;
            MestoOdrzavanja = mestoOdrzavanja;
        }

        public string GeografskaDuzina { get; set; }
        public string GeografskaSirina { get; set; }
        public MestoOdrzavanja MestoOdrzavanja { get; set; }
    }
}