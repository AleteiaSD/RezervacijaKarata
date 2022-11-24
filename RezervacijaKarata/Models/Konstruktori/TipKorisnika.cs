using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RezervacijaKarata.Models
{
    public enum ImeTipa { ZLATNI, SREBRNI, BRONZANI}
    public class TipKorisnika
    {
        public TipKorisnika() { }
        public TipKorisnika(ImeTipa imeTipa,float popust, float trazeniBrojBodova)
        {
            ImeTipa = imeTipa;
            Popust = popust;
            TrazeniBrojBodova = trazeniBrojBodova;
        }

        public ImeTipa ImeTipa { get; set; }
        public float Popust { get; set; }
        public float TrazeniBrojBodova { get; set; }
    }
}