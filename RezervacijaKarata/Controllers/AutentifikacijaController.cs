using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RezervacijaKarata.Models;
using RezervacijaKarata.Models.CuvanjePodataka;
using RezervacijaKarata.Models.Konstruktori;

namespace RezervacijaKarata.Controllers
{
    public class AutentifikacijaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult ObradiRegistraciju()
        {
            Manifestacija korisnik = new Manifestacija();
            Session["korisnik"] = korisnik;
            return View(korisnik);
        }
        [HttpPost]
        public ActionResult ObradiRegistraciju(Korisnik korisnik)
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];

            string korisnickoImePolje = Request["korisnickoIme"];
            string lozinkaPolje = Request["lozinka"];
            string imePolje = Request["ime"];
            string prezimePolje = Request["prezime"];
            string polPolje = Request["pol"];
            string datumPolje = Request["datumRodjenja"];

            //validacija 
            if (
                korisnickoImePolje == "" || lozinkaPolje == "" || imePolje == "" || prezimePolje == "" || polPolje.ToString() == "" || datumPolje.ToString() == "")
            {
                ViewBag.Message = $"Popunite sva polja ispravno!";
                return View("Registracija");
            }

            //1999 / 01 / 13
            //FROMAT DATUMA dd/MM/yyyy
            char[] delimeters = { ',', '.', '-', '_', '/' };
            string datum = korisnik.DatumRodjenja;
            string formatDatuma = "";
            string[] splitovanDatum = datum.Split(delimeters);
            if (splitovanDatum[1].Length > 1)
                formatDatuma = splitovanDatum[0] + "/" + splitovanDatum[1] + "/" + splitovanDatum[2];
            else
                formatDatuma = splitovanDatum[0] + "/0" + splitovanDatum[1] + "/" + splitovanDatum[2];



            foreach (Korisnik kor in korisnici.listaKorisnika.Values)
            {
                if (korisnik.KorisnickoIme == kor.KorisnickoIme)
                {
                    ViewBag.Message = $"Korisnik sa imenom: {korisnik.KorisnickoIme} vec postoji!";
                    return View("Registracija");
                }

            }
            korisnik.DatumRodjenja = formatDatuma;
            korisnik.Uloga = KorisnikUloga.KUPAC;
            korisnik.TipKorisnika = new TipKorisnika(ImeTipa.BRONZANI, 1000, 1000);
            korisnici.listaKorisnika.Add(korisnik.KorisnickoIme, korisnik);
            CuvanjePodatakaOKorisniku.SacuvajKorisnika(korisnik);
            Session["korisnik"] = korisnik;
            return RedirectToAction("Index", "Autentifikacija");
        }
        public ActionResult Registracija(Manifestacija korisnik)
        {
            return View();
        }

        public ActionResult ObradiLogovanje(string korisnickoIme, string lozinka)
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            string korisnickoImePolje = Request["korisnickoIme"];
            string lozinkaPolje = Request["lozinka"];
            foreach (Korisnik korisnik in korisnici.listaKorisnika.Values)
            {
                if (korisnik == null || korisnickoImePolje == "" || lozinkaPolje == "")
                {
                    ViewBag.Message = $"Korisnik nije registrovan ili je neispravno korisnicko ime ili lozinka!";
                    return View("Index");
                }
                if (korisnik.KorisnickoIme == korisnickoIme && korisnik.Lozinka == lozinkaPolje && korisnik.Uloga.ToString() == "ADMINISTRATOR")
                {
                    Session["korisnik"] = korisnik;
                    return RedirectToAction("Index", "Home");
                }
                if (korisnik.KorisnickoIme == korisnickoIme && korisnik.Lozinka == lozinkaPolje && korisnik.Uloga.ToString() == "KUPAC")
                {
                    Session["korisnik"] = korisnik;
                    return RedirectToAction("Index", "Home");
                }
                if (korisnik.KorisnickoIme == korisnickoIme && korisnik.Lozinka == lozinkaPolje && korisnik.Uloga.ToString() == "PRODAVAC")
                {
                    Session["korisnik"] = korisnik;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Index");
        }
        public ActionResult Profil()
        {
            Korisnik korisnik = new Korisnik();
            korisnik = (Korisnik)Session["korisnik"];

            if (korisnik == null || korisnik.KorisnickoIme == "" || korisnik.Lozinka == "")
            {
                ViewBag.Message = $"Niko nije ulogovan!";
                return View("Index");
            }
            if (korisnik.Uloga.ToString() == "ADMINISTRATOR")
            {
                return RedirectToAction("Administrator", "Administrator");
            }
            if (korisnik.Uloga.ToString() == "KUPAC")
            {
                return RedirectToAction("Korisnik", "Administrator");
            }
            if (korisnik.Uloga.ToString() == "PRODAVAC")
            {
                return RedirectToAction("Prodavac", "Administrator");
            }
            return View("Index");
        }
        public ActionResult IzlogujSe()
        {
            Session["korisnickoIme"] = "";
            Session["lozinka"] = "";
            return RedirectToAction("Index", "Autentifikacija");
        }
        public ActionResult Pocetna()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}