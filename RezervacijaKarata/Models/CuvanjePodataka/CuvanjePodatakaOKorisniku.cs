using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace RezervacijaKarata.Models.CuvanjePodataka
{
    public class CuvanjePodatakaOKorisniku
    {
        public Dictionary<string, Korisnik> listaKorisnika { get; set; }

        public CuvanjePodatakaOKorisniku(string putanja) //citam korisnika
        {
            putanja = HostingEnvironment.MapPath(putanja);
            listaKorisnika = new Dictionary<string, Korisnik>();
            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            
            while ((line = sr.ReadLine()) != null)
            {

                string[] tokeni = line.Split(';');
                ImeTipa tip = new ImeTipa();
                if (tokeni[7] == "BRONZANI") tip = ImeTipa.BRONZANI;
                if (tokeni[7] == "SREBRNI") tip = ImeTipa.SREBRNI;
                if (tokeni[7] == "ZLATNI") tip = ImeTipa.ZLATNI;
               
                Korisnik kor = new Korisnik(tokeni[0],
                                                tokeni[1],
                                                tokeni[2],
                                                tokeni[3],
                                                (KorisnikPol)Enum.Parse(typeof(KorisnikPol), tokeni[4]),
                                                tokeni[5],
                                                (KorisnikUloga)Enum.Parse(typeof(KorisnikUloga), tokeni[6]),
                                                new TipKorisnika(tip ,float.Parse(tokeni[8]),float.Parse(tokeni[9]) ),
                                                float.Parse(tokeni[10])

                                                );
                listaKorisnika.Add(kor.KorisnickoIme, kor);
            }
            sr.Close();
            stream.Close();
        }
        public static int idZaUpis = 20;
        public static void SacuvajKorisnika(Korisnik korisnik)//ovde ga upisujem u korisnici.txt
        {

            string putanja = HostingEnvironment.MapPath("~/App_Data/GlavneListe/korisnici.txt");
            FileStream stream = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);


            //FROMAT DATUMA dd/MM/yyyy
            char[] delimeters = { ',', '.', '-', '_', '/' };
            string datum = korisnik.DatumRodjenja;
            string formatDatuma = "";
            string[] splitovanDatum = datum.Split(delimeters);
            if (splitovanDatum[1].Length > 1)
                formatDatuma = splitovanDatum[0] + "/" + splitovanDatum[1] + "/" + splitovanDatum[2];
            else
                formatDatuma = splitovanDatum[0] + "/0" + splitovanDatum[1] + "/" + splitovanDatum[2];

            
            sw.WriteLine(
                            korisnik.KorisnickoIme          + ";" +
                            korisnik.Lozinka                + ";" +
                            korisnik.Ime                    + ";" +
                            korisnik.Prezime                + ";" +
                            korisnik.Pol                    + ";" +
                            formatDatuma                    + ";" +
                            korisnik.Uloga                  + ";" +
                            korisnik.TipKorisnika.ImeTipa   + ";" +
                            korisnik.TipKorisnika.Popust    + ";" +
                            korisnik.TipKorisnika.TrazeniBrojBodova +";"+
                            korisnik.BrojSakupljenihBodova + ";"
                            
                        );
            sw.Close();
            stream.Close();
        }
        public static void ObrisiSveKorisnike()
        {
            string putanja = HostingEnvironment.MapPath("~/App_Data/GlavneListe/korisnici.txt");
            FileStream stream = new FileStream(putanja, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            sw.Write("");
            sw.Close();
            stream.Close();
        }
    }
}