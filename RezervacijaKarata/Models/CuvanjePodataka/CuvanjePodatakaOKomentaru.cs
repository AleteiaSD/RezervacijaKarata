using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using static RezervacijaKarata.Models.Komentar;

namespace RezervacijaKarata.Models.CuvanjePodataka
{
    public class CuvanjePodatakaOKomentaru
    {
        public Dictionary<string, Komentar> listaKomentara { get; set; }

        public CuvanjePodatakaOKomentaru(string putanja) //citam korisnika
        {
            putanja = HostingEnvironment.MapPath(putanja);
            listaKomentara = new Dictionary<string, Komentar>();
            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {

                string[] tokeni = line.Split(';');         
                Komentar kom = new Komentar(    tokeni[0],
                                                tokeni[1],
                                                tokeni[2],
                                                tokeni[3],
                                                new Manifestacija(  tokeni[4],
                                                                    tokeni[5],
                                                                    (TipManifestacije)Enum.Parse(typeof(TipManifestacije), tokeni[6]),
                                                                    int.Parse(tokeni[7]),
                                                                    tokeni[8],
                                                                    tokeni[9],
                                                                    float.Parse(tokeni[10]),
                                                                    (Status)Enum.Parse(typeof(Status), tokeni[11]),
                                                                    new MestoOdrzavanja(tokeni[12].ToString(), tokeni[13].ToString(), tokeni[14].ToString()),
                                                                    tokeni[15]
                                                                    ),
                                                tokeni[16],
                                                int.Parse(tokeni[17]),
                                                (StanjeKomentara)Enum.Parse(typeof(StanjeKomentara), tokeni[18]),
                                                tokeni[19]
                                                );
                listaKomentara.Add(kom.IdKomentara, kom);
            }
            sr.Close();
            stream.Close();
        }
        public static void SacuvajKomentar(Komentar komentar)//ovde ga upisujem u korisnici.txt
        {

            string putanja = HostingEnvironment.MapPath("~/App_Data/GlavneListe/komentari.txt");
            FileStream stream = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.WriteLine(
                            komentar.IdKomentara+";"+
                            komentar.KorisnickoImeKupcaKarte+";"+
                            komentar.ImeKupcaKarte + ";" +
                            komentar.PrezimeKupcaKarte + ";" +
                            komentar.ManifestacijaKomentar.Id + ";" +
                            komentar.ManifestacijaKomentar.Naziv + ";" +
                            komentar.ManifestacijaKomentar.TipManifestacije + ";" +
                            komentar.ManifestacijaKomentar.BrojMesta + ";" +
                            komentar.ManifestacijaKomentar.DatumOdrzavanja + ";" +
                            komentar.ManifestacijaKomentar.VremeOdrzavanja + ";" +
                            komentar.ManifestacijaKomentar.CenaRegularKarte + ";" +
                            komentar.ManifestacijaKomentar.Status + ";" +
                            komentar.ManifestacijaKomentar.MestoOdrzavanja.UlicaIBroj + ";" +
                            komentar.ManifestacijaKomentar.MestoOdrzavanja.MestoGrad + ";" +
                            komentar.ManifestacijaKomentar.MestoOdrzavanja.PostanskiBroj + ";" +

                            komentar.TekstKomentara + ";" +
                           komentar.Ocena + ";" +
                           komentar.StanjeKomentar + ";" +
                           komentar.IdManifestacije+";"+

                           komentar.ManifestacijaKomentar.PosterManifestacije+";"


                           


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