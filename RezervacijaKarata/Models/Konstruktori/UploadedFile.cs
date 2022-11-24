using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RezervacijaKarata.Models.Konstruktori
{
    public class UploadedFile
    {
        private string _fileName;

        public string Filename
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private string _directoryPath;

        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }

        public UploadedFile(string filename, string directoryPath)
        {
            Filename = filename;
            DirectoryPath = directoryPath;
        }
    }
}