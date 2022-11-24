using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace RezervacijaKarata.Models.CuvanjePodataka
{
    public class CuvanjePodatakaOManifestacijama
    {
        public Dictionary<string, Manifestacija> listaManifestacija { get; set; }

        public CuvanjePodatakaOManifestacijama(string putanja) //citam korisnika
        {
            putanja = HostingEnvironment.MapPath(putanja);
            listaManifestacija = new Dictionary<string, Manifestacija>();
            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {

                string[] tokeni = line.Split(';');
                Manifestacija manifestacija = new Manifestacija(tokeni[0],
                                                                tokeni[1],
                                                                (TipManifestacije)Enum.Parse(typeof(TipManifestacije), tokeni[2]),
                                                                int.Parse(tokeni[3]),
                                                                tokeni[4],
                                                                tokeni[5],
                                                                float.Parse(tokeni[6]),
                                                                (Status)Enum.Parse(typeof(Status), tokeni[7]),
                                                                new MestoOdrzavanja(tokeni[8].ToString(),tokeni[9].ToString(),tokeni[10].ToString()),
                                                                tokeni[11]
                                                                );
                listaManifestacija.Add(manifestacija.Id, manifestacija);
            }
            sr.Close();
            stream.Close();
        }
        public static void SacuvajManifestaciju(Manifestacija manifestacija)//ovde ga upisujem u korisnici.txt
        {

            string putanja = HostingEnvironment.MapPath("~/App_Data/GlavneListe/manifestacije.txt");
            FileStream stream = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);


            //FROMAT DATUMA dd/MM/yyyy
            char[] delimeters = { ',', '.', '-', '_', '/' };
            string datum = manifestacija.DatumOdrzavanja;
            string formatDatuma = "";
            string[] splitovanDatum = datum.Split(delimeters);
            if (splitovanDatum[1].Length > 1)
                formatDatuma = splitovanDatum[2] + "/" + splitovanDatum[1] + "/" + splitovanDatum[0];
            else
                formatDatuma = splitovanDatum[2] + "/0" + splitovanDatum[1] + "/" + splitovanDatum[0];


            sw.WriteLine(
                            manifestacija.Id + ";" +
                            manifestacija.Naziv + ";" +
                            manifestacija.TipManifestacije + ";" +
                            manifestacija.BrojMesta + ";" +
                            formatDatuma + ";" +
                            manifestacija.VremeOdrzavanja + ";" +
                            manifestacija.CenaRegularKarte + ";" +
                            manifestacija.Status + ";" +
                            manifestacija.MestoOdrzavanja.UlicaIBroj + ";" +
                            manifestacija.MestoOdrzavanja.MestoGrad + ";" +
                            manifestacija.MestoOdrzavanja.PostanskiBroj + ";" +
                            manifestacija.PosterManifestacije+";"

                        );
            sw.Close();
            stream.Close();
        }
        public static void ObrisiSveManifestacije()
        {
            string putanja = HostingEnvironment.MapPath("~/App_Data/GlavneListe/manifestacije.txt");
            FileStream stream = new FileStream(putanja, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            sw.Write("");
            sw.Close();
            stream.Close();
        }
    }
}