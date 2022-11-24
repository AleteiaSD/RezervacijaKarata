using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RezervacijaKarata.Models.CuvanjePodataka;
using RezervacijaKarata.Models.Konstruktori;

namespace RezervacijaKarata
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CuvanjePodatakaOKorisniku korisnici = new CuvanjePodatakaOKorisniku("~/App_Data/GlavneListe/korisnici.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;

            CuvanjePodatakaOManifestacijama manifestacije = new CuvanjePodatakaOManifestacijama("~/App_Data/GlavneListe/manifestacije.txt");
            HttpContext.Current.Application["manifestacije"] = manifestacije;


            CuvanjePodatakaOKartama karte = new CuvanjePodatakaOKartama("~/App_Data/GlavneListe/karte.txt");
            HttpContext.Current.Application["karte"] = karte;

            CuvanjePodatakaOKomentaru komentari = new CuvanjePodatakaOKomentaru("~/App_Data/GlavneListe/komentari.txt");
            HttpContext.Current.Application["komentari"] = komentari;

            //za sliku
            string path = Path.Combine(Server.MapPath("~/Slike/"));
            List<UploadedFile> files = new List<UploadedFile>();
            foreach (string file in Directory.GetFiles(path))
            {
                files.Add(new UploadedFile(Path.GetFileName(file), file));
            }
            HttpContext.Current.Application["Slike"] = files;

        }
    }
}
