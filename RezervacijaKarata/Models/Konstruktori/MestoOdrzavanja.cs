using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RezervacijaKarata.Models
{
    public class MestoOdrzavanja
    {
        public MestoOdrzavanja()
        {
            UlicaIBroj = "";
            MestoGrad = "";
            PostanskiBroj = "";
        }
        public MestoOdrzavanja(string ulicaIBroj, string mestoGrad, string postanskiBroj)
        {
            UlicaIBroj = ulicaIBroj;
            MestoGrad = mestoGrad;
            PostanskiBroj = postanskiBroj;
        }

        public string UlicaIBroj { get; set; }
        public string MestoGrad { get; set; }
        public string PostanskiBroj { get; set; }

    }
}