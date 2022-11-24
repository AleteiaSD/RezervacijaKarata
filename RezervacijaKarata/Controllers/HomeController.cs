using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RezervacijaKarata.Models;
using RezervacijaKarata.Models.CuvanjePodataka;

namespace RezervacijaKarata.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CuvanjePodatakaOManifestacijama manifestacije = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            CuvanjePodatakaOManifestacijama aktivneManifestacije = new CuvanjePodatakaOManifestacijama("~/App_Data/LISTE/aktivneManifestacije.txt");
            foreach (Manifestacija m in manifestacije.listaManifestacija.Values)
            {
                if (m.Status.ToString() == "AKTIVNO")
                {
                    aktivneManifestacije.listaManifestacija.Add(m.Id, m);
                }
            }
            List<string> l1 = aktivneManifestacije.listaManifestacija.Keys.ToList();
            for (int i = 0; i < l1.Count - 1; i++)
            {
                for (int j = 1; j < l1.Count; j++)
                {
                    string[] splitovanDatum1 = aktivneManifestacije.listaManifestacija[l1[i]].DatumOdrzavanja.Split('/', ':', ' ');
                    string[] splitovanDatum2 = aktivneManifestacije.listaManifestacija[l1[j]].DatumOdrzavanja.Split('/', ':', ' ');
                    string[] splitovanoVreme1 = aktivneManifestacije.listaManifestacija[l1[i]].VremeOdrzavanja.Split(':');
                    string[] splitovanoVreme2 = aktivneManifestacije.listaManifestacija[l1[j]].VremeOdrzavanja.Split(':');

                    DateTime dateTime1 = new DateTime(int.Parse(splitovanDatum1[0]), int.Parse(splitovanDatum1[1] ), int.Parse(splitovanDatum1[2]), int.Parse(splitovanoVreme1[0]),int.Parse(splitovanoVreme1[1]),0);
                    DateTime dateTime2 = new DateTime(int.Parse(splitovanDatum2[0]), int.Parse(splitovanDatum2[1]), int.Parse(splitovanDatum2[2]), int.Parse(splitovanoVreme2[0]), int.Parse(splitovanoVreme2[1]), 0);
                    if (!(DateTime.Compare(dateTime1, dateTime2) <= 0))
                    {
                        Manifestacija temp = aktivneManifestacije.listaManifestacija[l1[i]];
                        aktivneManifestacije.listaManifestacija[l1[i]] = aktivneManifestacije.listaManifestacija[l1[j]];
                        aktivneManifestacije.listaManifestacija[l1[j]] = temp;  
                    }
                }
            }
            
            ViewBag.manifestacije = aktivneManifestacije;           
            return View();
        }
       
        public ActionResult PrikazJedne()
        {
            CuvanjePodatakaOManifestacijama manifestacije = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            CuvanjePodatakaOManifestacijama prikazJedneManifestacije = new CuvanjePodatakaOManifestacijama("~/App_Data/LISTE/prikazJedne.txt");

            CuvanjePodatakaOKomentaru k = (CuvanjePodatakaOKomentaru)HttpContext.Application["komentari"];
            CuvanjePodatakaOKomentaru kTemp = new CuvanjePodatakaOKomentaru("~/App_Data/LISTE/kTemp.txt");

            string id = Request["prikazHomeId"];
            foreach(Manifestacija m in manifestacije.listaManifestacija.Values)
            {
                if(m.Id == id && m.Status.ToString() =="AKTIVNO")
                {
                    prikazJedneManifestacije.listaManifestacija.Add(m.Id,m);
                }
            }
            ViewBag.manifestacije = prikazJedneManifestacije;

            
            foreach (Komentar kom in k.listaKomentara.Values)
            {
                if (kom.StanjeKomentar.ToString() == "ODOBREN" && kom.ManifestacijaKomentar.Naziv == prikazJedneManifestacije.listaManifestacija[id].Naziv)
                {
                    kTemp.listaKomentara.Add(kom.IdManifestacije, kom);
                }
            }
            ViewBag.komentari = kTemp;
            return View("PrikazJedne");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Pretraga()
        {
            CuvanjePodatakaOManifestacijama manifestacije = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            CuvanjePodatakaOManifestacijama pretragaManifestacije = new CuvanjePodatakaOManifestacijama("~/App_Data/LISTE/pretragaManifestacije.txt");
            Dictionary<string, Manifestacija> pomocnaLista = new Dictionary<string, Manifestacija>();
            foreach (Manifestacija m in manifestacije.listaManifestacija.Values)
            {
                if (m.Status.ToString() == "AKTIVNO")
                {

                    pomocnaLista.Add(m.Id, m);
                }
            }
            int a = 0, b = 0, c = 0, d = 0, e = 0;
            string pretragaNaziv = Request["pretragaNaziv"];
            if (pretragaNaziv != "") a = 1;

            string pretragaUlicaIBroj = Request["pretragaUlicaIBroj"];
            string pretragaMestoGrad = Request["pretragaMestoGrad"];
            string pretragaPostanskiBroj = Request["pretragaPostanskiBroj"];
            MestoOdrzavanja mestoOdrzavanja = new MestoOdrzavanja();
            if (pretragaUlicaIBroj != "" && pretragaMestoGrad != "" && pretragaPostanskiBroj.ToString() != "") 
            {                
                mestoOdrzavanja = new MestoOdrzavanja(pretragaUlicaIBroj, pretragaMestoGrad, pretragaPostanskiBroj);    b = 1;
            }
            else
            {
                ViewBag.GreskaMestoOdrzavanja = "Morate uneti sve podatke za mesto odrzavanja!";
            }


            string pretragaDatumOd = Request["pretragaDatumOd"];
            string pretragaDatumDo = Request["pretragaDatumDo"];
            if (pretragaDatumOd != "" && pretragaDatumDo != "") c = 1;


            string pretragaLokacija = Request["pretragaLokacija"];
            if (pretragaLokacija != "" ) d = 1;

            string pretragaCenaOd = Request["pretragaCenaOd"];
            string pretragaCenaDo = Request["pretragaCenaDo"];
            if (pretragaCenaOd != "" && pretragaCenaDo != "") e = 1;

            string pretragaSortiranje = Request["pretragaSortiranje"];
            string redosledSortiranja = Request["redosledSortiranja"];

            int i = 0;
            //5
            if (a == 1 && b == 1 && c == 1 && d == 1 && e == 1) i = 31;
            //4
            else if (a == 1 && b == 1 && c == 1 && d == 1 && e == 0) i = 26;
            else if (a == 1 && b == 1 && c == 1 && d == 0 && e == 1) i = 27;
            else if (a == 1 && b == 1 && c == 0 && d == 1 && e == 1) i = 28;
            else if (a == 1 && b == 0 && c == 1 && d == 1 && e == 1) i = 29;
            else if (a == 0 && b == 1 && c == 1 && d == 1 && e == 1) i = 30;
            //3
            else if (a == 1 && b == 1 && c == 1 && d == 0 && e == 0) i = 16;
            else if (a == 1 && b == 1 && c == 0 && d == 1 && e == 0) i = 17;
            else if (a == 1 && b == 1 && c == 0 && d == 0 && e == 1) i = 18;

            else if (a == 1 && b == 0 && c == 1 && d == 1 && e == 0) i = 19;
            else if (a == 1 && b == 0 && c == 1 && d == 0 && e == 1) i = 20;
            else if (a == 1 && b == 0 && c == 0 && d == 1 && e == 1) i = 21;

            else if (a == 0 && b == 1 && c == 1 && d == 1 && e == 0) i = 22;
            else if (a == 0 && b == 1 && c == 1 && d == 0 && e == 1) i = 23;
            else if (a == 0 && b == 1 && c == 0 && d == 1 && e == 1) i = 24;

            else if (a == 0 && b == 0 && c == 1 && d == 1 && e == 1) i = 25;
            //2
            else if (a == 1 && b == 1 && c == 0 && d == 0 && e == 0) i = 6;
            else if (a == 1 && b == 0 && c == 1 && d == 0 && e == 0) i = 7;
            else if (a == 1 && b == 0 && c == 0 && d == 1 && e == 0) i = 8;
            else if (a == 1 && b == 0 && c == 0 && d == 0 && e == 1) i = 9;
            else if (a == 0 && b == 1 && c == 1 && d == 0 && e == 0) i = 10;
            else if (a == 0 && b == 1 && c == 0 && d == 1 && e == 0) i = 11;
            else if (a == 0 && b == 1 && c == 0 && d == 0 && e == 1) i = 12;
            else if (a == 0 && b == 0 && c == 1 && d == 1 && e == 0) i = 13;
            else if (a == 0 && b == 0 && c == 1 && d == 0 && e == 1) i = 14;
            else if (a == 0 && b == 0 && c == 0 && d == 1 && e == 1) i = 15;
            //1
            else if (a == 1 && b == 0 && c == 0 && d == 0 && e == 0) i = 1;
            else if (a == 0 && b == 1 && c == 0 && d == 0 && e == 0) i = 2;
            else if (a == 0 && b == 0 && c == 1 && d == 0 && e == 0) i = 3;
            else if (a == 0 && b == 0 && c == 0 && d == 1 && e == 0) i = 4;
            else if (a == 0 && b == 0 && c == 0 && d == 0 && e == 1) i = 5;


            Dictionary<string, Manifestacija> temp = new Dictionary<string, Manifestacija>();
            Dictionary<string, Manifestacija> temp1 = new Dictionary<string, Manifestacija>();
            Dictionary<string, Manifestacija> temp2 = new Dictionary<string, Manifestacija>();
            Dictionary<string, Manifestacija> temp3= new Dictionary<string, Manifestacija>();
            Dictionary<string, Manifestacija> temp4 = new Dictionary<string, Manifestacija>();
            Dictionary<string, Manifestacija> temp5 = new Dictionary<string, Manifestacija>();
            switch (i)
            {
                //1
                case 1: temp = NazivManifestacije(pomocnaLista,pretragaNaziv); break;
                case 2: temp = MestoOdrzavanjaManifestacije(pomocnaLista,mestoOdrzavanja); break;
                case 3: temp = DatumManifestacije(pomocnaLista, pretragaDatumOd, pretragaDatumDo); break;
                case 4: temp = LokacijaManifestacije(pomocnaLista, pretragaLokacija); break;
                case 5: temp = CenaManifestacije(pomocnaLista,pretragaCenaOd,pretragaCenaDo); break;
                //2
                case 6: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                        temp = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja); break;
                case 7: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                        temp = DatumManifestacije(temp1, pretragaDatumOd, pretragaDatumDo);  break;
                case 8: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv); 
                        temp = LokacijaManifestacije(temp1, pretragaLokacija); break;
                case 9: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                        temp = CenaManifestacije(pomocnaLista, pretragaCenaOd, pretragaCenaDo); break;

                case 10: temp2 = MestoOdrzavanjaManifestacije(pomocnaLista, mestoOdrzavanja);
                         temp = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo); break;                        
                case 11: temp2 = MestoOdrzavanjaManifestacije(pomocnaLista, mestoOdrzavanja);
                         temp = LokacijaManifestacije(temp2, pretragaLokacija);  break;
                case 12: temp2 = MestoOdrzavanjaManifestacije(pomocnaLista, mestoOdrzavanja);
                         temp = CenaManifestacije(temp2, pretragaCenaOd, pretragaCenaDo); break;

                case 13: temp3 = DatumManifestacije(pomocnaLista, pretragaDatumOd, pretragaDatumDo);
                         temp = LokacijaManifestacije(temp3, pretragaLokacija); break;
                case 14: temp3 = DatumManifestacije(pomocnaLista, pretragaDatumOd, pretragaDatumDo);
                         temp = CenaManifestacije(temp3, pretragaCenaOd, pretragaCenaDo); break;
                case 15: temp4 = LokacijaManifestacije(pomocnaLista, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;
                //3
                case 16: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp2 = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja);
                         temp = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo); break;
                case 17: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp2 = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja);
                         temp = LokacijaManifestacije(temp2, pretragaLokacija); break;
                case 18: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp2 = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja);
                         temp = CenaManifestacije(temp2, pretragaCenaOd, pretragaCenaDo); break;

                case 19: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp3 = DatumManifestacije(temp1, pretragaDatumOd, pretragaDatumDo);
                         temp = LokacijaManifestacije(temp3, pretragaLokacija); break;
                case 20: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp3 = DatumManifestacije(temp1, pretragaDatumOd, pretragaDatumDo);
                         temp = CenaManifestacije(temp3, pretragaCenaOd, pretragaCenaDo); break;

                case 21: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp4 = LokacijaManifestacije(temp1, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;

                case 22: temp2 = MestoOdrzavanjaManifestacije(pomocnaLista, mestoOdrzavanja);
                         temp3 = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo);
                         temp = LokacijaManifestacije(temp3, pretragaLokacija); break;
                case 23: temp2 = MestoOdrzavanjaManifestacije(pomocnaLista, mestoOdrzavanja);
                         temp3 = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo);
                         temp = CenaManifestacije(temp3, pretragaCenaOd, pretragaCenaDo); break;
                case 24: temp2 = MestoOdrzavanjaManifestacije(pomocnaLista, mestoOdrzavanja);
                         temp4 = LokacijaManifestacije(temp2, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;
                case 25: temp3 = DatumManifestacije(pomocnaLista, pretragaDatumOd, pretragaDatumDo);
                         temp4 = LokacijaManifestacije(temp3, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;

                //4
                case 26: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp2 = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja);
                         temp3 = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo);
                         temp = LokacijaManifestacije(temp3, pretragaLokacija); break;
                case 27: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp2 = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja);
                         temp3 = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo);
                         temp = CenaManifestacije(temp3, pretragaCenaOd, pretragaCenaDo); break;
                case 28: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp2 = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja);
                         temp4 = LokacijaManifestacije(temp2, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;
                case 29: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp3 = DatumManifestacije(temp1, pretragaDatumOd, pretragaDatumDo);
                         temp4 = LokacijaManifestacije(temp3, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;
                case 30: temp2 = MestoOdrzavanjaManifestacije(pomocnaLista, mestoOdrzavanja);
                         temp3 = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo);
                         temp4 = LokacijaManifestacije(temp3, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;
                //5
                case 31: temp1 = NazivManifestacije(pomocnaLista, pretragaNaziv);
                         temp2 = MestoOdrzavanjaManifestacije(temp1, mestoOdrzavanja);
                         temp3 = DatumManifestacije(temp2, pretragaDatumOd, pretragaDatumDo);
                         temp4 = LokacijaManifestacije(temp3, pretragaLokacija);
                         temp = CenaManifestacije(temp4, pretragaCenaOd, pretragaCenaDo); break;
            }

            
            if (pretragaSortiranje == "Naziv manifestacije")
            {
                if (redosledSortiranja == "Rastuci")
                {
                    SortirajPoNazivuRastuce(temp,pretragaManifestacije.listaManifestacija);
                }
                else if(redosledSortiranja=="Opadajuci")
                {
                    SortirajPoNazivuOpadajuce(temp,pretragaManifestacije.listaManifestacija);
                }
            }
            else if (pretragaSortiranje == "Datum i vreme odrzavanja")
            {
                if (redosledSortiranja == "Rastuci")
                {
                    SortirajPoDatumuIVremenuRastuce(temp, pretragaManifestacije.listaManifestacija);
                }
                else if (redosledSortiranja == "Opadajuci")
                {
                    SortirajPoDatumuIVremenuOpadajuce(temp, pretragaManifestacije.listaManifestacija);
                }
            }
            else if (pretragaSortiranje == "Ceni karte")
            {
                if (redosledSortiranja == "Rastuci")
                {
                    SortirajPoCeniRastuce(temp, pretragaManifestacije.listaManifestacija);
                }
                else if (redosledSortiranja == "Opadajuci")
                {
                    SortirajPoCeniOpadajuce(temp, pretragaManifestacije.listaManifestacija);
                }
            }
            else if (pretragaSortiranje == "Mesto odrzavanja")
            {
                if (redosledSortiranja == "Rastuci")
                {
                    SortirajPoMestuOdrzavanjaRastuce(temp, pretragaManifestacije.listaManifestacija);
                }
                else if (redosledSortiranja == "Opadajuci")
                {
                    SortirajPoMestuOdrzavanjaOpadajuce(temp, pretragaManifestacije.listaManifestacija);
                }
            }
            else
            {
                foreach (Manifestacija m in temp.Values)
                {
                    pretragaManifestacije.listaManifestacija.Add(m.Id, m);
                }
            }
            
            ViewBag.manifestacije = pretragaManifestacije;
            return View("Pretraga");
        }
        public Dictionary<string, Manifestacija> NazivManifestacije(Dictionary<string, Manifestacija> temp, string pretragaNaziv)
        {
            Dictionary<string, Manifestacija> temp0 = new Dictionary<string, Manifestacija>();//privremena lista
            foreach (Manifestacija m in temp.Values)
            {
                if (m.Naziv == pretragaNaziv)
                {
                    temp0.Add(m.Id, m); //tu mi se nalaze sad svi sa nazivom
                }
            }

            return temp0;
        }

        public Dictionary<string, Manifestacija> MestoOdrzavanjaManifestacije(Dictionary<string, Manifestacija> temp, MestoOdrzavanja mestoOdrzavanja)
        {
            Dictionary<string, Manifestacija> temp1 = new Dictionary<string, Manifestacija>();//privremena lista
            foreach (Manifestacija m in temp.Values)
            {
                if (m.MestoOdrzavanja.UlicaIBroj == mestoOdrzavanja.UlicaIBroj && m.MestoOdrzavanja.MestoGrad == mestoOdrzavanja.MestoGrad && m.MestoOdrzavanja.PostanskiBroj == mestoOdrzavanja.PostanskiBroj)
                {
                    temp1.Add(m.Id, m);
                }
            }
            return temp1;
        }
        public Dictionary<string, Manifestacija> DatumManifestacije(Dictionary<string, Manifestacija> temp, string pretragaDatumOd, string pretragaDatumDo)
        {

            char[] delimeters = { ',', '.', '-', '_', '/' };
            string[] splitovanDatumOd = pretragaDatumOd.Split(delimeters);
            string[] splitovanDatumDo = pretragaDatumDo.Split(delimeters);

            DateTime dateTime1 = DateTime.Parse(splitovanDatumOd[1] + "-" + splitovanDatumOd[2] + "-" + splitovanDatumOd[0]);
            DateTime dateTime2 = DateTime.Parse(splitovanDatumDo[1] + "-" + splitovanDatumDo[2] + "-" + splitovanDatumDo[0]);
            Dictionary<string, Manifestacija> temp2 = new Dictionary<string, Manifestacija>();//privremena lista
            foreach (Manifestacija m in temp.Values)
            {
                string[] splitovanoTrenutniIzListe = m.DatumOdrzavanja.Split(delimeters);
                DateTime trenutniDatum = DateTime.Parse(splitovanoTrenutniIzListe[1] + "-" + splitovanoTrenutniIzListe[2] + "-" + splitovanoTrenutniIzListe[0]);
                if (dateTime1 <= trenutniDatum && dateTime2 >= trenutniDatum)
                {                 
                    temp2.Add(m.Id, m); // tu mi se nalaze sad svi na lokaciji                          
                }
            }
            return temp2;
        }
        public Dictionary<string, Manifestacija> LokacijaManifestacije(Dictionary<string, Manifestacija> temp, string pretragaLokacijaGrad)
        {
            Dictionary<string, Manifestacija> temp3 = new Dictionary<string, Manifestacija>();//privremena lista

            foreach (Manifestacija m in temp.Values)
            {
                if (m.MestoOdrzavanja.MestoGrad == pretragaLokacijaGrad)
                {
                    temp3.Add(m.Id, m); // tu mi se nalaze sad svi na lokaciji                          
                }
            }
            return temp3;
        }
        public Dictionary<string, Manifestacija> CenaManifestacije(Dictionary<string, Manifestacija> temp, string pretragaCenaOd, string pretragaCenaDo)
        {
              Dictionary<string, Manifestacija> temp4 = new Dictionary<string, Manifestacija>();//privremena lista
              foreach (Manifestacija m in temp.Values)
              {
                   if (int.Parse(pretragaCenaOd) <= m.CenaRegularKarte && int.Parse(pretragaCenaDo) >= m.CenaRegularKarte)
                   {
                       temp4.Add(m.Id, m); // tu mi se nalaze sad svi na lokaciji
                   }
              }
            return temp4;                    
        }


        //SORTIRANJE
        public void SortirajPoNazivuRastuce(Dictionary<string,Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {
            
            foreach (Manifestacija man in temp.Values)
            {
                tempCpy.Add(man.Id, man);
            }

            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(tempCpy[l1[i]].Naziv.ToString(), tempCpy[l1[j]].Naziv.ToString());
                    if (tmp < 1)
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }
                }
            }

        }
        public void SortirajPoNazivuOpadajuce(Dictionary<string, Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {
            foreach (Manifestacija man in temp.Values)
            {
                tempCpy.Add(man.Id, man);
            }

            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(tempCpy[l1[i]].Naziv.ToString(), tempCpy[l1[j]].Naziv.ToString());
                    if (tmp > 0)
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }
                }
            }
        }

        public void SortirajPoDatumuIVremenuRastuce(Dictionary<string, Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {
            foreach (Manifestacija man in temp.Values)
            {
                tempCpy.Add(man.Id, man);
            }
            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count-1; i++)
            {
                for (int j = 1; j <= l1.Count-1; j++)
                {
                    string[] splitovanDatum1 = tempCpy[l1[i]].DatumOdrzavanja.Split('/');
                    string[] splitovanDatum2 = tempCpy[l1[j]].DatumOdrzavanja.Split('/');
                    string[] splitovanoVreme1 = tempCpy[l1[i]].VremeOdrzavanja.Split(':');
                    string[] splitovanoVreme2 = tempCpy[l1[j]].VremeOdrzavanja.Split(':');
                    DateTime date1 = new DateTime(int.Parse(splitovanDatum1[0]), int.Parse(splitovanDatum1[1]), int.Parse(splitovanDatum1[2]), int.Parse(splitovanoVreme1[0]), int.Parse(splitovanoVreme1[1]),0);
                    DateTime date2 = new DateTime(int.Parse(splitovanDatum2[0]), int.Parse(splitovanDatum2[1]), int.Parse(splitovanDatum2[2]), int.Parse(splitovanoVreme2[0]), int.Parse(splitovanoVreme2[1]), 0);
                    if (!(DateTime.Compare(date1,date2) <= 0))
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }
                    
                }
            }
        }
        public void SortirajPoDatumuIVremenuOpadajuce(Dictionary<string, Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {
            foreach (Manifestacija man in temp.Values)
            {
                tempCpy.Add(man.Id, man);
            }
            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count - 1; i++)
            {
                for (int j = 1; j < l1.Count; j++)
                {
                    string[] splitovanDatum1 = tempCpy[l1[i]].DatumOdrzavanja.Split('/');
                    string[] splitovanDatum2 = tempCpy[l1[j]].DatumOdrzavanja.Split('/');
                    string[] splitovanoVreme1 = tempCpy[l1[i]].VremeOdrzavanja.Split(':');
                    string[] splitovanoVreme2 = tempCpy[l1[j]].VremeOdrzavanja.Split(':');
                    DateTime date1 = new DateTime(int.Parse(splitovanDatum1[0]), int.Parse(splitovanDatum1[1]), int.Parse(splitovanDatum1[2]), int.Parse(splitovanoVreme1[0]), int.Parse(splitovanoVreme1[1]), 0);
                    DateTime date2 = new DateTime(int.Parse(splitovanDatum2[0]), int.Parse(splitovanDatum2[1]), int.Parse(splitovanDatum2[2]), int.Parse(splitovanoVreme2[0]), int.Parse(splitovanoVreme2[1]), 0);
                    if (!(DateTime.Compare(date1, date2) > 0))
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }

                }
            }
        }

        public void SortirajPoCeniRastuce(Dictionary<string, Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {

            foreach (Manifestacija m in temp.Values)
            {
                tempCpy.Add(m.Id, m);
            }

            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    if (tempCpy[l1[i]].CenaRegularKarte < tempCpy[l1[j]].CenaRegularKarte)
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }
                }
            }
        }
        public void SortirajPoCeniOpadajuce(Dictionary<string, Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {

            foreach (Manifestacija m in temp.Values)
            {
                tempCpy.Add(m.Id, m);
            }

            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    if (tempCpy[l1[i]].CenaRegularKarte > tempCpy[l1[j]].CenaRegularKarte)
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }
                }
            }
        }
        public void SortirajPoMestuOdrzavanjaRastuce(Dictionary<string, Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {
            foreach (Manifestacija man in temp.Values)
            {
                tempCpy.Add(man.Id, man);
            }

            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(tempCpy[l1[i]].MestoOdrzavanja.ToString(), tempCpy[l1[j]].MestoOdrzavanja.ToString());
                    if (tmp > 0 )
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }
                }
            }
        }
        public void SortirajPoMestuOdrzavanjaOpadajuce(Dictionary<string, Manifestacija> temp, Dictionary<string, Manifestacija> tempCpy)
        {
            foreach (Manifestacija man in temp.Values)
            {
                tempCpy.Add(man.Id, man);
            }

            List<string> l1 = tempCpy.Keys.ToList();
            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l1.Count; j++)
                {
                    int tmp = String.Compare(tempCpy[l1[i]].MestoOdrzavanja.ToString(), tempCpy[l1[j]].MestoOdrzavanja.ToString());
                    if (tmp < 1)
                    {
                        Manifestacija prom = tempCpy[l1[i]];
                        tempCpy[l1[i]] = tempCpy[l1[j]];
                        tempCpy[l1[j]] = prom;
                    }
                }
            }
        }
        
    }
}







/*  static string idStatic = "";
        //KUPOVINA i REZERVISANJE KARTE
        public ActionResult KupiKartuIliRezervisi()
        {
            CuvanjePodatakaOManifestacijama manifestacije = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            CuvanjePodatakaOManifestacijama manifestacijaTemp = new CuvanjePodatakaOManifestacijama("~/App_Data/LISTE/rezervisiTemp.txt");

            foreach (Manifestacija m in manifestacije.listaManifestacija.Values)
            {
                manifestacijaTemp.listaManifestacija.Add(m.Id, m);
            }

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/karteKupovina.txt");
            string kupiKatruId = Request["kupiKatruId"];
            string kupiKatruVrsta = Request["kupiKatruVrsta"];
            string kupiKatruBrojKarata = Request["kupiKatruBrojKarata"];
            int brojKarata = Convert.ToInt32(kupiKatruBrojKarata);
            Karta ka = new Karta();
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik == null)
            {
                ViewBag.Message = $"Karte mogu da rezervisu i kupe samo kupci!";
                return RedirectToAction("Index", "Autentifikacija");
            }
            if (korisnik.Uloga.ToString() == "KUPAC")
            {
                if (Request.Form["rezervisi"] != null)
                {
                    Random r = new Random();
                    int id = r.Next(100000000) + 1000000000;
                    foreach (Manifestacija m in manifestacijaTemp.listaManifestacija.Values)
                    {
                        float cena=0;
                        if (m.Id == kupiKatruId && m.Status == Status.AKTIVNO)
                        {
                            if (korisnik.TipKorisnika.ImeTipa.ToString() == "BRONZANI")//10% popusta
                            {
                                cena = ((float)brojKarata * m.CenaRegularKarte) * (float)0.1;
                            }
                            else if (korisnik.TipKorisnika.ImeTipa.ToString() == "SREBRNI")//20% popusta
                            {
                                cena = ((float)brojKarata * m.CenaRegularKarte) * (float)0.2;
                            }
                            if (korisnik.TipKorisnika.ImeTipa.ToString() == "ZLATNI")//30% popusta
                            {
                                cena = ((float)brojKarata * m.CenaRegularKarte) * (float)0.3;
                            }                    
                            string datumVreme = m.DatumOdrzavanja + " " + m.VremeOdrzavanja;
                            Tip tip = new Tip();
                            switch (kupiKatruVrsta)
                            {
                                case "REGULAR": tip = Tip.REGULAR; break;
                                case "FAN PIT": tip = Tip.FANPIT; break;
                                case "VIP": tip = Tip.VIP; break;
                            }
                                Karta k = new Karta(id.ToString(), m, datumVreme, cena, korisnik.Ime, korisnik.Prezime, StatusKarta.REZERVISANA, tip, brojKarata, korisnik.KorisnickoIme);
                                karte.listaKarata.Add(id.ToString(),k);
                                CuvanjePodatakaOKartama.SacuvajKartu(new Karta(id.ToString(), m, datumVreme, cena, korisnik.Ime, korisnik.Prezime, StatusKarta.REZERVISANA, tip,brojKarata,korisnik.KorisnickoIme));
                                idStatic = id.ToString();
                            
                            ViewBag.karte = karte;
                            return View("PotvrdaRezervacije");
                        }
                    }
                }
            }
            return View("Index");
        }      
 *  
 *  public ActionResult PotvrdaRezervacije()
        {
            CuvanjePodatakaOManifestacijama manifestacije = (CuvanjePodatakaOManifestacijama)HttpContext.Application["manifestacije"];
            CuvanjePodatakaOManifestacijama manifestacijaTemp = new CuvanjePodatakaOManifestacijama("~/App_Data/LISTE/rezervisiTemp.txt");

            foreach (Manifestacija m in manifestacije.listaManifestacija.Values)
            {
                manifestacijaTemp.listaManifestacija.Add(m.Id, m);
            }

            CuvanjePodatakaOKartama karte = (CuvanjePodatakaOKartama)HttpContext.Application["karte"];
            CuvanjePodatakaOKartama karteTemp = new CuvanjePodatakaOKartama("~/App_Data/LISTE/karteKupovina.txt");

            foreach(Karta k in karte.listaKarata.Values)
            {
                karteTemp.listaKarata.Add(k.JedinstveniIdentifikatorKarte, k);
            }
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            CuvanjePodatakaOKorisniku koris = (CuvanjePodatakaOKorisniku)HttpContext.Application["korisnici"];
            CuvanjePodatakaOKorisniku korisniciTemp = new CuvanjePodatakaOKorisniku("~/App_Data/LISTE/korisniciTemp.txt");
            foreach(Korisnik k in koris.listaKorisnika.Values)
            {
                korisniciTemp.listaKorisnika.Add(k.KorisnickoIme,k);
            }

            if (Request.Form["potvrdiRezervaciju"] != null)
            {
                int broj=0;
                foreach(Manifestacija m in manifestacijaTemp.listaManifestacija.Values)
                {
                    if(m.Id == karteTemp.listaKarata[idStatic].ManifestacijaZaKojuJeRezervisana.Id)
                    {
                        broj = m.BrojMesta - karteTemp.listaKarata[idStatic].BrojKarata;
                        manifestacijaTemp.listaManifestacija[m.Id].BrojMesta = broj;
                        float rez = 0;
                        float pom = karteTemp.listaKarata[idStatic].Cena;
                        rez += pom / 1000 * 133;
                        korisnik.BrojSakupljenihBodova += (float)rez;

                        if (korisnik.Uloga.ToString() == "KUPAC" && korisnik.BrojSakupljenihBodova > 4000)
                        {
                            TipKorisnika tip = new TipKorisnika();
                            tip = new TipKorisnika(ImeTipa.SREBRNI, (float)0.2, (float)8000);
                            CuvanjePodatakaOKorisniku.ObrisiSveKorisnike();
                            foreach(Korisnik k in korisniciTemp.listaKorisnika.Values)
                            {                               
                                if(k.KorisnickoIme == korisnik.KorisnickoIme)
                                {
                                    k.TipKorisnika = tip;
                                }
                            }
                            foreach(Korisnik k in korisniciTemp.listaKorisnika.Values)
                            {
                                CuvanjePodatakaOKorisniku.SacuvajKorisnika(k);
                            }
                        }
                        if (korisnik.Uloga.ToString() == "KUPAC" && korisnik.BrojSakupljenihBodova > 8000)
                        {
                            TipKorisnika tip = new TipKorisnika();
                            tip = new TipKorisnika(ImeTipa.ZLATNI, (float)0.3, (float)8000);
                            CuvanjePodatakaOKorisniku.ObrisiSveKorisnike();
                            foreach (Korisnik k in korisniciTemp.listaKorisnika.Values)
                            {
                                if (k.KorisnickoIme == korisnik.KorisnickoIme)
                                {
                                    k.TipKorisnika = tip;
                                }
                            }
                            foreach (Korisnik k in korisniciTemp.listaKorisnika.Values)
                            {
                                CuvanjePodatakaOKorisniku.SacuvajKorisnika(k);
                            }
                        }
                    }
                }

                korisnik.SveKarteBezObziraNaStatus = karte.listaKarata;
                karte.listaKarata.Remove(idStatic);

                CuvanjePodatakaOManifestacijama.ObrisiSveManifestacije();
                foreach (Manifestacija m in manifestacijaTemp.listaManifestacija.Values)
                {
                    CuvanjePodatakaOManifestacijama.SacuvajManifestaciju(m);
                }
                ViewBag.manifestacije = manifestacijaTemp;
                
            }
            else if(Request.Form["odustaniOdRezervacije"]!=null)
            {
                
                karte.listaKarata[idStatic].StatusKarta = StatusKarta.ODUSTANAK;
                korisnik.BrojSakupljenihBodova -= karte.listaKarata[idStatic].Cena / 1000 * 133 * 4 * karte.listaKarata[idStatic].BrojKarata;
                CuvanjePodatakaOKartama.ObrisiSveKarte();
                foreach(Karta k in karte.listaKarata.Values)
                {
                    CuvanjePodatakaOKartama.SacuvajKartu(k);
                }
                CuvanjePodatakaOManifestacijama.ObrisiSveManifestacije();
                foreach (Manifestacija m in manifestacijaTemp.listaManifestacija.Values)
                {
                    CuvanjePodatakaOManifestacijama.SacuvajManifestaciju(m);
                }
                ViewBag.manifestacije = manifestacijaTemp;
            }
            return View("Index");
        }*/
