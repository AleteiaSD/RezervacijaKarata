using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RezervacijaKarata.Models
{
    public class Komentar
    {
        public enum StanjeKomentara { ODOBREN, ODBIJEN}
        public Komentar() { }
        public Komentar(string idKomentara, string korisnickoImeKupcaKarte, string imeKupcaKarte, string prezimeKupcaKarte, Manifestacija manifestacijaKomentar, string tekstKomentara, int ocena,StanjeKomentara stanjeKomenara, string idManifestacije)
        {
            IdKomentara = idKomentara;
            KorisnickoImeKupcaKarte = korisnickoImeKupcaKarte;
            ImeKupcaKarte = imeKupcaKarte;
            PrezimeKupcaKarte = prezimeKupcaKarte;
            ManifestacijaKomentar = manifestacijaKomentar;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            StanjeKomentar = stanjeKomenara;
            IdManifestacije=idManifestacije;
        }

        public string IdKomentara { get; set; }
        public string KorisnickoImeKupcaKarte { get; set; }
        public string ImeKupcaKarte { get; set; }
        public string PrezimeKupcaKarte { get; set; }
        public Manifestacija ManifestacijaKomentar { get; set; }
        public string TekstKomentara { get; set; }
        public int Ocena{ get; set; } //od 1 do 5
        public StanjeKomentara StanjeKomentar { get; set; }
        public string IdManifestacije { get; set; }

    }
}