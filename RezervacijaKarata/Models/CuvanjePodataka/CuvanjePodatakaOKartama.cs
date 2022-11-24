using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace RezervacijaKarata.Models.CuvanjePodataka
{
    public class CuvanjePodatakaOKartama
    {
        public Dictionary<string, Karta> listaKarata { get; set; }

        public CuvanjePodatakaOKartama(string putanja) //citam kartu
        {
            putanja = HostingEnvironment.MapPath(putanja);
            listaKarata = new Dictionary<string, Karta>();
            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {

                string[] tokeni = line.Split(';');               
                Karta karta = new Karta(        tokeni[0],
                                                new Manifestacija(  tokeni[1],
                                                                    tokeni[2],
                                                                    (TipManifestacije)Enum.Parse(typeof(TipManifestacije), tokeni[3]),
                                                                    int.Parse(tokeni[4]),
                                                                    tokeni[5],
                                                                    tokeni[6],
                                                                    float.Parse(tokeni[7]),
                                                                    (Status)Enum.Parse(typeof(Status), tokeni[8]),
                                                                    new MestoOdrzavanja(tokeni[9].ToString(), tokeni[10].ToString(), tokeni[11].ToString()),
                                                                    tokeni[12]
                                                                    ),
                                                tokeni[13],
                                                float.Parse(tokeni[14]),
                                                tokeni[15],
                                                tokeni[16],
                                                (StatusKarta)Enum.Parse(typeof(StatusKarta), tokeni[17]),
                                                (Tip)Enum.Parse(typeof(Tip),tokeni[18]),
                                                int.Parse(tokeni[19]),
                                                tokeni[20]
                                                );
                listaKarata.Add(karta.JedinstveniIdentifikatorKarte, karta);
            }
            sr.Close();
            stream.Close();
        }
        public static int idZaUpis = 20;
        public static void SacuvajKartu(Karta karta)//ovde ga upisujem u korisnici.txt
        {

            string putanja = HostingEnvironment.MapPath("~/App_Data/GlavneListe/karte.txt");
            FileStream stream = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);


            sw.WriteLine(
                            karta.JedinstveniIdentifikatorKarte + ";" +

                            karta.ManifestacijaZaKojuJeRezervisana.Id + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.Naziv + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.TipManifestacije + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.BrojMesta + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.DatumOdrzavanja + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.VremeOdrzavanja + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.CenaRegularKarte + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.Status + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.MestoOdrzavanja.UlicaIBroj + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.MestoOdrzavanja.MestoGrad + ";" +
                            karta.ManifestacijaZaKojuJeRezervisana.MestoOdrzavanja.PostanskiBroj + ";" +

                            karta.DatumIVremeManifestacije + ";" +
                            karta.Cena + ";" +
                            karta.ImeKupca + ";" +
                            karta.PrezimeKupca + ";" +
                            karta.StatusKarta + ";" +
                            karta.Tip + ";" +

                            karta.BrojKarata + ";" +
                            karta.KorisnickoIme + ";" +

                            karta.ManifestacijaZaKojuJeRezervisana.PosterManifestacije+";"

                        );
            sw.Close();
            stream.Close();
        }
        public static void ObrisiSveKarte()
        {
            string putanja = HostingEnvironment.MapPath("~/App_Data/GlavneListe/karte.txt");
            FileStream stream = new FileStream(putanja, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            sw.Write("");
            sw.Close();
            stream.Close();
        }
    }
}