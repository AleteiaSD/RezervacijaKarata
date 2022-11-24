using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RezervacijaKarata.Models.CuvanjePodataka;

namespace RezervacijaKarata.Models
{
    public enum KorisnikPol { MUSKO, ZENSKO}
    public enum KorisnikUloga{ ADMINISTRATOR, PRODAVAC, KUPAC, OBRISAN}
  
    public class Korisnik:Controller
    {
        public Korisnik() { }
        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, KorisnikPol pol, string datumRodjenja, KorisnikUloga uloga, TipKorisnika tipKorisnika,float brojBodova)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            TipKorisnika = tipKorisnika;
            BrojSakupljenihBodova = brojBodova;
        }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public KorisnikPol Pol { get; set; }
        public string DatumRodjenja { get; set; }
        public KorisnikUloga Uloga { get; set; }
        public TipKorisnika TipKorisnika { get; set; }


        public Dictionary<string, Karta> SveKarteBezObziraNaStatus { get; set; }
        public float BrojSakupljenihBodova { get; set; }
        public Dictionary<string,Manifestacija> ManifestacijeKorisnika { get; set; }
    }
}