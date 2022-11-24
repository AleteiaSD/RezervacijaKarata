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
    public class AdministratorController : Controller
    {
        public ActionResult Administrator()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];           
            ViewBag.korisnici = korisnici;
            
            return View("Administrator");
        }
        public ActionResult Korisnik()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);                    
                }
            }
            ViewBag.karte = karteTemp;            
            return View("Korisnik");
        }
       
        public ActionResult DodajKomentar()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            string idManifestacijeKomentar = Request["idManifestacijeKomentar"];
            string ostaviKomentar = Request["ostaviKomentar"];
            string oceniKomentar = Request["oceniKomentar"];

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            CuvanjePodatakaOKomentaru komentarTemps = new CuvanjePodatakaOKomentaru("~/App_Data/LISTE/komentariTemp.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];

          
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                    if (k.StatusKarta.ToString() == "REZERVISANA" && DateTime.Parse(k.DatumIVremeManifestacije) < DateTime.Now && k.ManifestacijaZaKojuJeRezervisana.Id == idManifestacijeKomentar)
                    {
                        Random temp = new Random();
                        int id = temp.Next(1999);
                        int idKomentara = temp.Next(1000000)+id;
                        
                        CuvanjePodatakaOKomentaru.SacuvajKomentar(new Komentar(
                                                                                idKomentara.ToString(),
                                                                                k.KorisnickoIme,
                                                                                k.ImeKupca, 
                                                                                k.PrezimeKupca, 
                                                                                k.ManifestacijaZaKojuJeRezervisana, 
                                                                                ostaviKomentar,
                                                                                int.Parse(oceniKomentar),
                                                                                Komentar.StanjeKomentara.ODBIJEN,
                                                                                k.ManifestacijaZaKojuJeRezervisana.Id));
                    }
                }
            }
            ViewBag.karte = karteTemp;
            return View("Korisnik");
        }
        public ActionResult OdobriKomentarProdAdmin()
        {
            CuvanjePodatakaOKomentaru k = (CuvanjePodatakaOKomentaru)HttpContext.Application["komentari"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            string IDizmeniStanjeKomentara = Request["IDizmeniStanjeKomentara"];

            if (korisnik.Uloga.ToString() == "PRODAVAC" || korisnik.Uloga.ToString() == "ADMINISTRATOR")
            {
                k.listaKomentara[IDizmeniStanjeKomentara].StanjeKomentar = Komentar.StanjeKomentara.ODOBREN;
                
            }
            else
            {
                ViewBag.OdobriKomentarPoruka = $"Nemate administratorska prava za ovu funkciju!";

            }
            ViewBag.komentari = k;
            return View("Komentari");
        }

        public ActionResult OdobriKomentar()
        {
            CuvanjePodatakaOKomentaru k = (CuvanjePodatakaOKomentaru)HttpContext.Application["komentari"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik.Uloga.ToString() == "PRODAVAC" || korisnik.Uloga.ToString() == "ADMINISTRATOR")
            {
                ViewBag.komentari = k;
            }
            else
            {
                CuvanjePodatakaOKomentaru kTemp = new CuvanjePodatakaOKomentaru("~/App_Data/LISTE/kTemp.txt");
                foreach (Komentar kom in k.listaKomentara.Values)
                {
                    if(kom.StanjeKomentar.ToString()== "ODOBREN")
                    {
                        kTemp.listaKomentara.Add(kom.IdKomentara, kom);
                    }
                }
                ViewBag.komentari = kTemp;
            }
            return View("Komentari");
        }
       
        public ActionResult PrikazOdobrenihKomentaraKupac()
        {
            CuvanjePodatakaOKomentaru k = (CuvanjePodatakaOKomentaru)HttpContext.Application["komentari"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik.Uloga.ToString() == "PRODAVAC" || korisnik.Uloga.ToString() == "ADMINISTRATOR")
            {
                ViewBag.komentari = k;
            }
            else
            {
                CuvanjePodatakaOKomentaru kTemp = new CuvanjePodatakaOKomentaru("~/App_Data/LISTE/kTemp.txt");
                foreach (Komentar kom in k.listaKomentara.Values)
                {
                    if (kom.StanjeKomentar.ToString() == "ODOBREN" && kom.KorisnickoImeKupcaKarte == korisnik.KorisnickoIme)
                    {
                        kTemp.listaKomentara.Add(kom.IdKomentara, kom);
                    }
                }
                ViewBag.komentari = kTemp;
            }

            return View("KomentariTabela");
        }
        public ActionResult PrikazOdobrenihKomentara()
        {
            CuvanjePodatakaOKomentaru k = (CuvanjePodatakaOKomentaru)HttpContext.Application["komentari"];
            CuvanjePodatakaOKomentaru kTemp = new CuvanjePodatakaOKomentaru("~/App_Data/LISTE/kTemp.txt");
            foreach (Komentar kom in k.listaKomentara.Values)
            {
                if (kom.StanjeKomentar.ToString() == "ODOBREN")
                {
                    kTemp.listaKomentara.Add(kom.IdKomentara, kom);
                }
            }
            ViewBag.komentari = kTemp;
            return View("KomentariTabela");
        }
        public ActionResult Prodavac()
        {
            CuvanjePodatakaOManifestacijama m = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            ViewBag.manifestacije = m;
            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.StatusKarta.ToString() == "REZERVISANA")
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            ViewBag.karte = karteTemp;
            return View("Prodavac");
        }
        public ActionResult ObrisiKorisnikaLogicki()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisniciTemp = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/obrisiKorisnika.txt");
            string obrisiKorisnickoIme = Request["obrisiKorisnickoIme"];
            foreach (Korisnik k in korisnici.listaKorisnika.Values)
            {
                if(obrisiKorisnickoIme ==k.KorisnickoIme)
                {
                    KorisnikUloga uloga = new KorisnikUloga();
                    uloga = KorisnikUloga.OBRISAN;
                    korisnici.listaKorisnika[k.KorisnickoIme].Uloga = uloga;
                }
            }
            CuvanjePodatakaOKorisniku.ObrisiSveKorisnike();
            foreach(Korisnik k in korisnici.listaKorisnika.Values)
            {
                CuvanjePodatakaOKorisniku.SacuvajKorisnika(k);
            }
            ViewBag.korisnici = korisnici;
            return View("Administrator");
        }
        public ActionResult IzmeniKorisnika()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            Dictionary<string, Korisnik> korisniciCopy = new Dictionary<string, Korisnik>();

            korisniciCopy = korisnici.listaKorisnika; //napravljena privremena kopija podataka

            string korisnickoIme = Request["izmeniKorisnickoIme"];
            string lozinka = Request["izmeniLozinku"];
            string ime = Request["izmeniIme"];
            string prezime = Request["izmeniPrezime"];
            string pol = Request["pol"];
            string datum = Request["izmeniDatumRodjenja"];
            KorisnikPol kp = new KorisnikPol();
            if (pol == "MUSKO") kp = KorisnikPol.MUSKO;
            else if (pol == "ZENSKO") kp = KorisnikPol.ZENSKO;

            Korisnik trenutniKorisnik = new Korisnik();
            trenutniKorisnik = (Korisnik)Session["korisnik"];
            //FROMAT DATUMA dd/MM/yyyy
            char[] delimeters = { ',', '.', '-', '_', '/' };
            string formatDatuma = "";
            string[] splitovanDatum = datum.Split(delimeters);
            if (splitovanDatum[1].Length > 1)
                formatDatuma = splitovanDatum[0] + "/" + splitovanDatum[1] + "/" + splitovanDatum[2];
            else
                formatDatuma = splitovanDatum[0] + "/0" + splitovanDatum[1] + "/" + splitovanDatum[2];


            Korisnik izmenjenKorisnik = new Korisnik(korisnickoIme,lozinka,ime,prezime,kp, formatDatuma, KorisnikUloga.KUPAC,new TipKorisnika(trenutniKorisnik.TipKorisnika.ImeTipa,trenutniKorisnik.TipKorisnika.Popust,trenutniKorisnik.TipKorisnika.TrazeniBrojBodova),trenutniKorisnik.BrojSakupljenihBodova);


            foreach(Korisnik korisnik in korisnici.listaKorisnika.Values)
            {
                if(trenutniKorisnik.KorisnickoIme==korisnik.KorisnickoIme)
                {
                    CuvanjePodatakaOKorisniku.ObrisiSveKorisnike();

                    korisniciCopy[trenutniKorisnik.KorisnickoIme] = izmenjenKorisnik; 

                    foreach(Korisnik k in korisniciCopy.Values)
                    {
                        CuvanjePodatakaOKorisniku.SacuvajKorisnika(k);
                    }                    
                    return RedirectToAction("Korisnik", "Administrator");
                }
            }

            return RedirectToAction("Korisnik", "Administrator");
        }

//===========================================SORTIRANJE==================================================================
        public ActionResult SortirajPoImenuRastuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(korisnikListaSort.listaKorisnika[l1[i]].Ime.ToString(), korisnikListaSort.listaKorisnika[l1[j]].Ime.ToString());
                    if (tmp < 1)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }
           
            ViewBag.korisnici = korisnikListaSort;
            
            return View("Administrator");
        }
        public ActionResult SortirajPoImenuOpadajuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(korisnikListaSort.listaKorisnika[l1[i]].Ime.ToString(), korisnikListaSort.listaKorisnika[l1[j]].Ime.ToString());
                    if (tmp > 0)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }

            ViewBag.korisnici = korisnikListaSort;
            return View("Administrator");
        }

        public ActionResult SortirajPoPrezimenuRastuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(korisnikListaSort.listaKorisnika[l1[i]].Prezime.ToString(), korisnikListaSort.listaKorisnika[l1[j]].Prezime.ToString());
                    if (tmp < 1)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }

            ViewBag.korisnici = korisnikListaSort;
            return View("Administrator");
        }
        public ActionResult SortirajPoPrezimenuOpadajuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(korisnikListaSort.listaKorisnika[l1[i]].Prezime.ToString(), korisnikListaSort.listaKorisnika[l1[j]].Prezime.ToString());
                    if (tmp > 0)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }

            ViewBag.korisnici = korisnikListaSort;
            return View("Administrator");
        }

        public ActionResult SortirajPoKorisnickomImenuRastuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(korisnikListaSort.listaKorisnika[l1[i]].KorisnickoIme.ToString(), korisnikListaSort.listaKorisnika[l1[j]].KorisnickoIme.ToString());
                    if (tmp < 1)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }

            ViewBag.korisnici = korisnikListaSort;
            return View("Administrator");
        }
        public ActionResult SortirajPoKorisnickomImenuOpadajuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(korisnikListaSort.listaKorisnika[l1[i]].KorisnickoIme.ToString(), korisnikListaSort.listaKorisnika[l1[j]].KorisnickoIme.ToString());
                    if (tmp > 0)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }

            ViewBag.korisnici = korisnikListaSort;
            return View("Administrator");
        }
        public ActionResult SortirajPoBrojSakupljenihBodovaRastuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    if (korisnikListaSort.listaKorisnika[l1[i]].BrojSakupljenihBodova > korisnikListaSort.listaKorisnika[l1[j]].BrojSakupljenihBodova)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }

            ViewBag.korisnici = korisnikListaSort;

            return View("Administrator");
        }
        public ActionResult SortirajPoBrojSakupljenihBodovaOpadajuce()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisnikListaSort = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/sortiranje.txt");

            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                korisnikListaSort.listaKorisnika.Add(k.KorisnickoIme, k);
            }

            List<string> l1 = korisnikListaSort.listaKorisnika.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    if (korisnikListaSort.listaKorisnika[l1[i]].BrojSakupljenihBodova < korisnikListaSort.listaKorisnika[l1[j]].BrojSakupljenihBodova)
                    {
                        Korisnik temp = korisnikListaSort.listaKorisnika[l1[i]];
                        korisnikListaSort.listaKorisnika[l1[i]] = korisnikListaSort.listaKorisnika[l1[j]];
                        korisnikListaSort.listaKorisnika[l1[j]] = temp;
                    }
                }
            }

            ViewBag.korisnici = korisnikListaSort;

            return View("Administrator");
        }
//=======================PRETRAGA==========================================================================================

        public ActionResult PretragaIme()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisniciLista = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/pretraga.txt");
            string pretraga = Request["pretragaIme"];
            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                if (k.Ime.Equals(pretraga))
                {
                    korisniciLista.listaKorisnika.Add(k.KorisnickoIme, k);
                }
            }
            ViewBag.korisnici = korisniciLista;
            return View("Administrator");
        }
        public ActionResult PretragaPrezime()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisniciLista = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/pretraga.txt");
            string pretraga = Request["pretragaPrezime"];
            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                if (k.Prezime.Equals(pretraga))
                {
                    korisniciLista.listaKorisnika.Add(k.KorisnickoIme, k);
                }
            }
            ViewBag.korisnici = korisniciLista;
            return View("Administrator");
        }
        public ActionResult PretragaKorisnickoIme()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisniciLista = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/pretraga.txt");
            string pretraga = Request["pretragaKorisnickoIme"];
            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                if (k.KorisnickoIme.Equals(pretraga))
                {
                    korisniciLista.listaKorisnika.Add(k.KorisnickoIme, k);
                }
            }
            ViewBag.korisnici = korisniciLista;
            return View("Administrator");
        }
        //=======================FILTRIRANJE==========================================================================================
        public ActionResult FiltriranjeUloga()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku filtriranjeLista = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/filtriranje.txt");
            string filtriranje = Request["filtriranjeUloga"];
            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                if (k.Uloga.ToString() == filtriranje)
                {
                    filtriranjeLista.listaKorisnika.Add(k.KorisnickoIme, k);
                }
            }
            ViewBag.korisnici = filtriranjeLista;
            return View("Administrator");
        }
        public ActionResult FiltriranjeTipKorisnika()
        {
            CuvanjePodatakaOKorisniku korisnik = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku filtriranjeLista = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/filtriranje.txt");
            string filtriranje = Request["filtriranjeTipKorisnika"];
            foreach (Korisnik k in korisnik.listaKorisnika.Values)
            {
                if (k.TipKorisnika.ImeTipa.ToString() == filtriranje)
                {
                    filtriranjeLista.listaKorisnika.Add(k.KorisnickoIme, k);
                }
            }
            ViewBag.korisnici = filtriranjeLista;
            return View("Administrator");
        }
        //=============================KREIRAJ PRODAVCA===========================================================================
        
        public ActionResult ObrisiProdavca()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisniciTemp = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/obrisiKorisnika.txt");
            string obrisiKorisnickoIme = Request["obrisiKorisnickoIme"];
            foreach (Korisnik k in korisnici.listaKorisnika.Values)
            {
                if (obrisiKorisnickoIme == k.KorisnickoIme)
                {
                    KorisnikUloga uloga = new KorisnikUloga();
                    uloga = KorisnikUloga.OBRISAN;
                    korisnici.listaKorisnika[k.KorisnickoIme].Uloga = uloga;
                }
            }
            CuvanjePodatakaOKorisniku.ObrisiSveKorisnike();
            foreach (Korisnik k in korisnici.listaKorisnika.Values)
            {
                CuvanjePodatakaOKorisniku.SacuvajKorisnika(k);
            }
            ViewBag.korisnici = korisnici;
            return View("Administrator");
        }
        public ActionResult KreirajProdavca()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];

            string korisnickoImePolje = Request["kreirajKorisnickoIme"];
            string lozinkaPolje = Request["kreirajLozinku"];
            string imePolje = Request["kreirajIme"];
            string prezimePolje = Request["kreirajPrezime"];
            string polPolje = Request["kreirajpol"];
            string datumPolje = Request["kreirajDatumRodjenja"];

            //validacija 
            if (
                korisnickoImePolje == "" || lozinkaPolje == "" || imePolje == "" || prezimePolje == "" || polPolje.ToString() == "" || datumPolje.ToString() == "")
            {
                ViewBag.Message = $"Popunite sva polja ispravno!";
                return View("Administrator");
            }
            KorisnikPol pol = new KorisnikPol();
            if (polPolje == "MUSKO") pol = KorisnikPol.MUSKO;
            else if (polPolje == "ZENSKO") pol = KorisnikPol.ZENSKO;


            //FROMAT DATUMA dd/MM/yyyy
            char[] delimeters = { ',', '.', '-', '_', '/' , ' '};
            string datum = datumPolje;
            string formatDatuma = "";
            string[] splitovanDatum = datum.Split(delimeters);
            if (splitovanDatum[1].Length > 1)
                formatDatuma = splitovanDatum[0] + "/" + splitovanDatum[1] + "/" + splitovanDatum[2];
            else
                formatDatuma = splitovanDatum[0] + "/0" + splitovanDatum[1] + "/" + splitovanDatum[2];



            Korisnik prodavac = new Korisnik(korisnickoImePolje, lozinkaPolje, imePolje, prezimePolje, pol, formatDatuma, KorisnikUloga.PRODAVAC, new TipKorisnika(ImeTipa.BRONZANI,(float)1,1000),1000);
            foreach (Korisnik kor in korisnici.listaKorisnika.Values)
            {
                if (prodavac.KorisnickoIme == kor.KorisnickoIme)
                {
                    ViewBag.Message = $"Korisnik sa imenom: {prodavac.KorisnickoIme} vec postoji!";
                    return View("Registracija");
                }
            }
            korisnici.listaKorisnika.Add(prodavac.KorisnickoIme, prodavac);
            CuvanjePodatakaOKorisniku.SacuvajKorisnika(prodavac);
            Session["korisnik"] = prodavac;
            return RedirectToAction("Administrator", "Administrator");
        }
        //=========================DODAJ MANIFESTACIJU==========================
        
        public ActionResult ObrisiManifestaciju()
        {
            
            CuvanjePodatakaOManifestacijama manifestacija = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            string obrisiManifestacijuId = Request["obrisiManifestacijuId"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik.Uloga.ToString() == "ADMINISTRATOR")
            {
                foreach (Manifestacija m in manifestacija.listaManifestacija.Values)
                {
                    if (korisnik.Uloga.ToString() == "ADMINISTRATOR" && m.Id == obrisiManifestacijuId)
                    {
                        Status status = new Status();
                        status = Status.OBRISANO;
                        manifestacija.listaManifestacija[m.Id].Status = status;
                    }
                }
            }
            else
            {
                ViewBag.ObrisiManifestaciju = $"Nemate administratorska prava za ovu funkciju!";
                CuvanjePodatakaOManifestacijama.ObrisiSveManifestacije();
                foreach (Manifestacija m in manifestacija.listaManifestacija.Values)
                {
                    CuvanjePodatakaOManifestacijama.SacuvajManifestaciju(m);
                }
                ViewBag.manifestacije = manifestacija;
                CuvanjePodatakaOKartama kartePrava = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
                CuvanjePodatakaOKartama karteTempPrava = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");

                foreach (Karta k in kartePrava.listaKarata.Values)
                {
                    karteTempPrava.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
                ViewBag.karte = karteTempPrava;
                return View("Prodavac");
            }
            CuvanjePodatakaOManifestacijama.ObrisiSveManifestacije();
            foreach(Manifestacija m in manifestacija.listaManifestacija.Values)
            {
                CuvanjePodatakaOManifestacijama.SacuvajManifestaciju(m);
            }
            ViewBag.manifestacije = manifestacija;
            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            
            foreach (Karta k in karte.listaKarata.Values)
            {
                 karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);                
            }
            ViewBag.karte = karteTemp;
            return View("Prodavac");
        }
        [HttpPost]
        public ActionResult DodajManifestaciju(HttpPostedFileBase file)
        {
            CuvanjePodatakaOManifestacijama manifestacija = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            List<UploadedFile> files = (List<UploadedFile>)HttpContext.Application["files"];

            string izmeniId = Request["izmeniId"];
            string dodajNaziv = Request["dodajNaziv"];
            string dodajTipManifestacije = Request["dodajTipManifestacije"];
            string dodajBrojMesta = Request["dodajBrojMesta"];
            string dodajDatumOdrzavanja = Request["dodajDatumOdrzavanja"];
            string dodajVremeOdrzavanja = Request["dodajVremeOdrzavanja"];
            string dodajCenuRegularKarte = Request["dodajCenuRegularKarte"];
            string dodajUlicuIBroj = Request["dodajUlicuIBroj"];
            string dodajMestoGrad = Request["dodajMestoGrad"];

            string dodajPostanskiBroj = Request["dodajPostanskiBroj"];
            string slika = "";
            if (file.ContentLength > 0)
            {
                 slika = Path.GetFileName(file.FileName);
            }



            TipManifestacije tip = new TipManifestacije();
            if (dodajTipManifestacije == "KONCERT") tip = TipManifestacije.KONCERT;
            else if (dodajTipManifestacije == "FESTIVAL") tip = TipManifestacije.FESTIVAL;
            else if (dodajTipManifestacije == "POZORISTE") tip = TipManifestacije.POZORISTE;
            MestoOdrzavanja mestoOdrzavanja = new MestoOdrzavanja(dodajUlicuIBroj, dodajMestoGrad, dodajPostanskiBroj);


            //FROMAT DATUMA dd/MM/yyyy
            char[] delimeters = { ',', '.', '-', '_', '/' };
            string datum = dodajDatumOdrzavanja;
            string formatDatuma = "";
            string[] splitovanDatum = datum.Split(delimeters);
            if (splitovanDatum[1].Length > 1)
                formatDatuma = splitovanDatum[2] + "/" + splitovanDatum[1] + "/" + splitovanDatum[0];
            else
                formatDatuma = splitovanDatum[2] + "/0" + splitovanDatum[1] + "/" + splitovanDatum[0];


            if (Request.Form["dodajManifestaciju"] != null)
            {

                foreach(Manifestacija man in manifestacija.listaManifestacija.Values)
                    {
                        if (man.VremeOdrzavanja == dodajVremeOdrzavanja       && 
                            man.MestoOdrzavanja.UlicaIBroj == dodajUlicuIBroj &&
                            man.MestoOdrzavanja.MestoGrad == dodajMestoGrad   &&
                            man.MestoOdrzavanja.PostanskiBroj == dodajPostanskiBroj)
                        {
                            ViewBag.DodajManifestaciju = $"Postoji zakazana manifestacija u tom vremenu na toj lokaciji. Ne mozete dodati manifestaciju!";
                            ViewBag.manifestacije = manifestacija;
                        //dodaj karte
                            CuvanjePodatakaOKartama karteGreska = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
                            CuvanjePodatakaOKartama karteTempGreska = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
                            Korisnik korisnikGreska = (Korisnik)Session["korisnik"];
                            foreach (Karta k in karteGreska.listaKarata.Values)
                            {
                                if (k.StatusKarta.ToString() == "REZERVISANA")
                                {
                                karteTempGreska.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                }
                            }
                            ViewBag.karte = karteTempGreska;
                            return View("Prodavac");
                        }
                    }
            Ponovo://labela za ponovno kreiranje id
                Random temp = new Random();
                int idManifestacije = temp.Next(1000);

                foreach (Manifestacija man in manifestacija.listaManifestacija.Values)
                {
                    if (man.Id == idManifestacije.ToString())
                    {
                        goto Ponovo; // proveravam da li ima novih 
                    }
                }

               
                Manifestacija m = new Manifestacija(idManifestacije.ToString(), dodajNaziv, tip, int.Parse(dodajBrojMesta), formatDatuma, dodajVremeOdrzavanja, float.Parse(dodajCenuRegularKarte), Status.NEAKTIVNO, mestoOdrzavanja, slika);

                manifestacija.listaManifestacija.Add(m.Id, m);
                CuvanjePodatakaOManifestacijama.SacuvajManifestaciju(m);
                ViewBag.manifestacije = manifestacija;
            }
            else if (Request.Form["izmeniManifestaciju"] != null)
            {

                Manifestacija izmenjenaManifestacija = new Manifestacija(izmeniId, dodajNaziv, tip, int.Parse(dodajBrojMesta), formatDatuma, dodajVremeOdrzavanja, float.Parse(dodajCenuRegularKarte), Status.NEAKTIVNO, mestoOdrzavanja,slika);

                manifestacija.listaManifestacija[izmeniId] = izmenjenaManifestacija;
                //CuvanjePodatakaOManifestacijama.SacuvajManifestaciju(izmenjenaManifestacija);
                ViewBag.manifestacije = manifestacija;
            }
            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.StatusKarta.ToString() == "REZERVISANA")
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            ViewBag.karte = karteTemp;
            return View("Prodavac");
        }

        public ActionResult PredjiNaProdavca()
        {
            CuvanjePodatakaOManifestacijama manifestacija = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            ViewBag.manifestacije = manifestacija;
            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);                
            }
            ViewBag.karte = karteTemp;
            return View("Prodavac");
        }
        public ActionResult OdobriManifestaciju()
        {

            string izmeniIdStatus = Request["izmeniIdStatus"]; 

            CuvanjePodatakaOManifestacijama manifestacija = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            Korisnik trenutniKorisnik = new Korisnik();
            trenutniKorisnik = (Korisnik)Session["korisnik"];

            if ( trenutniKorisnik.Uloga.ToString() == "ADMINISTRATOR")
            {
                manifestacija.listaManifestacija[izmeniIdStatus].Status = Status.AKTIVNO;
            }
            else
            {
                ViewBag.OdobriManifestaciju = $"Nemate administratorska prava za ovu funkciju!";
                
            }
          
            ViewBag.manifestacije = manifestacija;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);                
            }
            ViewBag.karte = karteTemp;

            return View("Prodavac");
        }
        //KORISNIK SORTIRANJE
        public ActionResult SortirajPoNazivuRastuce()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            List<string> l1 = karteTemp.listaKarata.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {

                    int tmp = String.Compare(karteTemp.listaKarata[l1[i]].ManifestacijaZaKojuJeRezervisana.Naziv,karteTemp.listaKarata[l1[j]].ManifestacijaZaKojuJeRezervisana.Naziv);
                    if (tmp <1)
                    {
                        Karta temp = karteTemp.listaKarata[l1[i]];
                        karteTemp.listaKarata[l1[i]] = karteTemp.listaKarata[l1[j]];
                        karteTemp.listaKarata[l1[j]] = temp;
                    }
                }
            }

            ViewBag.karte = karteTemp;
            
            return View("Korisnik");
        }
        public ActionResult SortirajPoNazivuOpadajuce()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            List<string> l1 = karteTemp.listaKarata.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {

                    int tmp = String.Compare(karteTemp.listaKarata[l1[i]].ManifestacijaZaKojuJeRezervisana.Naziv, karteTemp.listaKarata[l1[j]].ManifestacijaZaKojuJeRezervisana.Naziv);
                    if (tmp >0)
                    {
                        Karta temp = karteTemp.listaKarata[l1[i]];
                        karteTemp.listaKarata[l1[i]] = karteTemp.listaKarata[l1[j]];
                        karteTemp.listaKarata[l1[j]] = temp;
                    }
                }
            }

            ViewBag.karte = karteTemp;
           
            return View("Korisnik");
        }


        public ActionResult SortirajPoCeniRastuce()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            List<string> l1 = karteTemp.listaKarata.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    if (karteTemp.listaKarata[l1[i]].Cena < karteTemp.listaKarata[l1[j]].Cena)
                    {
                        Karta temp = karteTemp.listaKarata[l1[i]];
                        karteTemp.listaKarata[l1[i]] = karteTemp.listaKarata[l1[j]];
                        karteTemp.listaKarata[l1[j]] = temp;
                    }
                }
            }

            ViewBag.karte = karteTemp;
            
            return View("Korisnik");
        }

        public ActionResult SortirajPoCeniOpadajuce()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            List<string> l1 = karteTemp.listaKarata.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    if (karteTemp.listaKarata[l1[i]].Cena > karteTemp.listaKarata[l1[j]].Cena)
                    {
                        Karta temp = karteTemp.listaKarata[l1[i]];
                        karteTemp.listaKarata[l1[i]] = karteTemp.listaKarata[l1[j]];
                        karteTemp.listaKarata[l1[j]] = temp;
                    }
                }
            }

            ViewBag.karte = karteTemp;
            
            return View("Korisnik");
        }

        public ActionResult SortirajPoDatumuRastuce()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            List<string> l1 = karteTemp.listaKarata.Keys.ToList();
            for (int i = 0; i < l1.Count - 1; i++)
            {
                for (int j = 1; j < l1.Count; j++)
                {
                    string[] splitovanDatum1 = karteTemp.listaKarata[l1[i]].DatumIVremeManifestacije.Split('/',':',' ');
                    string[] splitovanDatum2 = karteTemp.listaKarata[l1[j]].DatumIVremeManifestacije.Split('/',':',' ');

                    DateTime dateTime1 = DateTime.Parse(splitovanDatum1[2] + "-" + splitovanDatum1[1] + "-" + splitovanDatum1[0] +" "+splitovanDatum1[3]+":"+splitovanDatum1[4]);
                    DateTime dateTime2 = DateTime.Parse(splitovanDatum2[2] + "-" + splitovanDatum2[1] + "-" + splitovanDatum2[0] + " " + splitovanDatum2[3] + ":" + splitovanDatum2[4]);
                    
                    if (!(DateTime.Compare(dateTime1, dateTime2) <0 || DateTime.Compare(dateTime1, dateTime2)==0))
                    {
                        Karta temp = karteTemp.listaKarata[l1[i]];
                        karteTemp.listaKarata[l1[i]] = karteTemp.listaKarata[l1[j]];
                        karteTemp.listaKarata[l1[j]] = temp;
                    }
                }
            }

            ViewBag.karte = karteTemp;
            
            return View("Korisnik");
        }
        public ActionResult SortirajPoDatumuOpadajuce()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            foreach (Karta k in karte.listaKarata.Values)
            {
                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                }
            }
            List<string> l1 = karteTemp.listaKarata.Keys.ToList();
            for (int i = 0; i < l1.Count - 1; i++)
            {
                for (int j = 1; j < l1.Count; j++)
                {
                    string[] splitovanDatum1 = karteTemp.listaKarata[l1[i]].DatumIVremeManifestacije.Split('/', ':', ' ');
                    string[] splitovanDatum2 = karteTemp.listaKarata[l1[j]].DatumIVremeManifestacije.Split('/', ':', ' ');

                    DateTime dateTime1 = DateTime.Parse(splitovanDatum1[2] + "-" + splitovanDatum1[1] + "-" + splitovanDatum1[0] + " " + splitovanDatum1[3] + ":" + splitovanDatum1[4]);
                    DateTime dateTime2 = DateTime.Parse(splitovanDatum2[2] + "-" + splitovanDatum2[1] + "-" + splitovanDatum2[0] + " " + splitovanDatum2[3] + ":" + splitovanDatum2[4]);

                    if (DateTime.Compare(dateTime1, dateTime2) < 0)
                    {
                        Karta temp = karteTemp.listaKarata[l1[i]];
                        karteTemp.listaKarata[l1[i]] = karteTemp.listaKarata[l1[j]];
                        karteTemp.listaKarata[l1[j]] = temp;
                    }
                }
            }

            ViewBag.karte = karteTemp;
           
            return View("Korisnik");
        }

        
        public ActionResult PretragaKorisnickihRezervacija()
        {
            CuvanjePodatakaOKorisniku korisnici = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            ViewBag.korisnici = korisnici;

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/prikaziKarteZaKorisnika.txt");
            CuvanjePodatakaOKartama karteTempFilter = new CuvanjePodatakaOKartama("~/App_Data/LISTE/filterZaRezervaciju.txt");
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            int a=0, b=0, c=0;
            string pretragaRezervacijaNaziv = Request["pretragaRezervacijaNaziv"]; if (pretragaRezervacijaNaziv != "" && pretragaRezervacijaNaziv != null) a = 1;
            string pretragaRezervacijaCenaOd = Request["pretragaRezervacijaCenaOd"]; 
            string pretragaRezervacijaCenaDo = Request["pretragaRezervacijaCenaDo"]; if (pretragaRezervacijaCenaOd != "" && pretragaRezervacijaCenaOd != null &&
                                                                                          pretragaRezervacijaCenaDo != "" && pretragaRezervacijaCenaDo != null) b = 1;
            string pretragaRezervacijaDatumOd = Request["pretragaRezervacijaDatumOd"];
            string pretragaRezervacijaDatumDo = Request["pretragaRezervacijaDatumDo"]; if (pretragaRezervacijaDatumOd != "" && pretragaRezervacijaDatumOd != null &&
                                                                                        pretragaRezervacijaDatumDo != "" && pretragaRezervacijaDatumDo != null) c = 1;
            int i = 0;
            //1
            if (a == 1 && b == 0 && c == 0) i = 1;
            else if (a == 0 && b == 1 && c == 0) i = 2;
            else if (a == 0 && b == 0 && c == 1) i = 3;
            //2
            else if (a == 1 && b == 1 && c == 0) i = 4;
            else if (a == 1 && b == 0 && c == 1) i = 5;
            else if (a == 0 && b == 1 && c == 1) i = 6;
            //3
            else if (a == 1 && b == 1 && c == 1) i = 7;
            if (Request.Form["pretragaPretrazi"] != null)
            {
                foreach (Karta k in karte.listaKarata.Values)
                {
                    if (i != 0)
                    {
                        if (k.KorisnickoIme == korisnik.KorisnickoIme)
                        {
                            switch (i)
                            {
                                case 1:
                                    if (pretragaRezervacijaNaziv == k.ManifestacijaZaKojuJeRezervisana.Naziv)
                                    {
                                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                    }
                                    break;
                                case 2:
                                    if (float.Parse(pretragaRezervacijaCenaOd) <= k.Cena && float.Parse(pretragaRezervacijaCenaDo) >= k.Cena)
                                    {
                                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                    }
                                    break;
                                case 3:
                                    if (DateTime.Parse(pretragaRezervacijaDatumOd) < DateTime.Parse(k.DatumIVremeManifestacije) &&
                                        DateTime.Parse(pretragaRezervacijaDatumDo) > DateTime.Parse(k.DatumIVremeManifestacije))
                                    {
                                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                    }
                                    break;

                                case 4:
                                    if ((pretragaRezervacijaNaziv == k.ManifestacijaZaKojuJeRezervisana.Naziv) &&
                                        (float.Parse(pretragaRezervacijaCenaOd) <= k.Cena && float.Parse(pretragaRezervacijaCenaDo) >= k.Cena))
                                    {
                                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                    }
                                    break;
                                case 5:
                                    if ((pretragaRezervacijaNaziv == k.ManifestacijaZaKojuJeRezervisana.Naziv) &&
                                        (DateTime.Parse(pretragaRezervacijaDatumOd) <= DateTime.Parse(k.DatumIVremeManifestacije) &&
                                        DateTime.Parse(pretragaRezervacijaDatumDo) >= DateTime.Parse(k.DatumIVremeManifestacije)))
                                    {
                                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                    }
                                    break;
                                case 6:
                                    if ((float.Parse(pretragaRezervacijaCenaOd) <= k.Cena && float.Parse(pretragaRezervacijaCenaDo) >= k.Cena) &&
                                        (DateTime.Parse(pretragaRezervacijaDatumOd) <= DateTime.Parse(k.DatumIVremeManifestacije) &&
                                        DateTime.Parse(pretragaRezervacijaDatumDo) >= DateTime.Parse(k.DatumIVremeManifestacije)))
                                    {
                                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                    }
                                    break;
                                case 7:
                                    if ((pretragaRezervacijaNaziv == k.ManifestacijaZaKojuJeRezervisana.Naziv) &&
                                        (float.Parse(pretragaRezervacijaCenaOd) <= k.Cena && float.Parse(pretragaRezervacijaCenaDo) >= k.Cena) &&
                                        (DateTime.Parse(pretragaRezervacijaDatumOd) <= DateTime.Parse(k.DatumIVremeManifestacije) &&
                                        DateTime.Parse(pretragaRezervacijaDatumDo) >= DateTime.Parse(k.DatumIVremeManifestacije)))
                                    {
                                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                                    }
                                    break;
                            }

                        }
                    }

                }
                ViewBag.karte = karteTemp;
            }
            else if (Request.Form["filtrirajRezervacije"] != null)
            {
                string filtriranjeTip = Request["filtriranjeTip"];
                string filtriranjeStatus = Request["filtriranjeStatus"];

                foreach (Karta k in karte.listaKarata.Values)
                {
                    if (k.KorisnickoIme == korisnik.KorisnickoIme)
                    {
                        if (k.Tip.ToString() == filtriranjeTip && k.StatusKarta.ToString() == filtriranjeStatus)
                        {
                            karteTempFilter.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                        }
                        else if (k.Tip.ToString() == filtriranjeTip)
                        {
                            karteTempFilter.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                        }
                        else if (k.StatusKarta.ToString() == filtriranjeStatus)
                        {
                            karteTempFilter.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                        }
                    }
                }
                ViewBag.karte = karteTempFilter;
            }
            else if (Request.Form["prikazRezervisani"] != null)
            {
                foreach (Karta k in karte.listaKarata.Values)
                {
                    if(k.StatusKarta.ToString() == "REZERVISANA")
                    {
                        karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
                    }
                }
                ViewBag.karte = karteTemp;
            }
            else if (Request.Form["odustaniRezervisani"] != null)
            {
                string odustanakOdRezervacije = Request["odustanakOdRezervacije"];

                //FROMAT DATUMA dd/MM/yyyy
                char[] delimeters = { ',', '.', '-', '_', '/' , ' '};
                string datum = karte.listaKarata[odustanakOdRezervacije].DatumIVremeManifestacije;
                
                string[] splitovanDatum = datum.Split(delimeters);

                DateTime date = new DateTime(int.Parse(splitovanDatum[0]), int.Parse(splitovanDatum[1]), int.Parse(splitovanDatum[2]));

                if (DateTime.Now.AddDays(7)< date) //datum? format
                {
                    karte.listaKarata[odustanakOdRezervacije].StatusKarta = StatusKarta.ODUSTANAK;
                    korisnik.BrojSakupljenihBodova -= karte.listaKarata[odustanakOdRezervacije].Cena / 1000 * 133 * 4 * karte.listaKarata[odustanakOdRezervacije].BrojKarata;
                }
                else
                {
                    ViewBag.Odustanak = "Ne mozete odustati.";
                }

                CuvanjePodatakaOKartama.ObrisiSveKarte();
                foreach (Karta k in karte.listaKarata.Values)
                {
                    CuvanjePodatakaOKartama.SacuvajKartu(k);
                }
                ViewBag.karte = karte;
            }
            return View("Korisnik");
        }
        
    }
}