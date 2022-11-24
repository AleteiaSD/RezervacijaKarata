using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RezervacijaKarata.Models
{
    public enum StatusKarta { REZERVISANA,ODUSTANAK}
    public enum Tip { VIP,REGULAR,FANPIT}
    public class Karta
    {
        public Karta() { }
        public Karta(string jedinstveniIdentifikatorKarte, Manifestacija manifestacijaZaKojuJeRezervisana, string datumIVremeManifestacije, float cena, string imeKupca, string prezimeKupca, StatusKarta statusKarta, Tip tip,int brojKarata,string korisnickoIme)
        {
            JedinstveniIdentifikatorKarte = jedinstveniIdentifikatorKarte;
            ManifestacijaZaKojuJeRezervisana = manifestacijaZaKojuJeRezervisana;
            DatumIVremeManifestacije = datumIVremeManifestacije;
            Cena = cena;
            ImeKupca = imeKupca;
            PrezimeKupca = prezimeKupca;
            StatusKarta = statusKarta;
            Tip = tip;
            BrojKarata = brojKarata;
            KorisnickoIme = korisnickoIme;
        }

        public string JedinstveniIdentifikatorKarte { get; set; }
        public Manifestacija ManifestacijaZaKojuJeRezervisana { get; set; }
        public string DatumIVremeManifestacije { get; set; }
        public float Cena { get; set; }
        public string ImeKupca { get; set; }
        public string PrezimeKupca { get; set; }
        public StatusKarta StatusKarta { get; set; }
        public Tip Tip { get; set; }
        public int BrojKarata { get; set; }
        public string KorisnickoIme { get; set; }

    }
}