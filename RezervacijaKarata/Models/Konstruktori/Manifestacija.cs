using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RezervacijaKarata.Models.Konstruktori;

namespace RezervacijaKarata.Models
{
    public enum TipManifestacije { KONCERT, FESTIVAL, POZORISTE}
    public enum Status {AKTIVNO,NEAKTIVNO, OBRISANO}
    public class Manifestacija
    {
        public Manifestacija()
        {
        }

        public Manifestacija(string id, string naziv, TipManifestacije tipManifestacije, int brojMesta, string datumOdrzavanja,string vremeOdrzavanja, float cenaRegularKarte, Status status, MestoOdrzavanja mestoOdrzavanja, string poster)
        {
            Id = id;
            Naziv = naziv;
            TipManifestacije = tipManifestacije;
            BrojMesta = brojMesta;
            DatumOdrzavanja = datumOdrzavanja;
            VremeOdrzavanja = vremeOdrzavanja;
            CenaRegularKarte = cenaRegularKarte;
            Status = status;
            MestoOdrzavanja = mestoOdrzavanja;
            PosterManifestacije = poster;
        }
        public string Id { get; set; }
        public string Naziv { get; set; }
        public TipManifestacije TipManifestacije { get; set; }
        public int BrojMesta { get; set; }
        public string DatumOdrzavanja { get; set; }
        public string VremeOdrzavanja { get; set; }
        public float CenaRegularKarte { get; set; }
        public Status Status { get; set; }
        public MestoOdrzavanja MestoOdrzavanja { get; set; }
        public string Ocena { get; set; }
        public string Komentar { get; set; }


        public string PosterManifestacije { get; set; }

    }
}