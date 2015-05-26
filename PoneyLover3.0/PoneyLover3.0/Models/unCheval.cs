using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoneyLover3._0.Models
{
    public class unCheval
    {
        public int ID_ { get; set; }
        public string Nom_ { get; set; }
        public string Description_ { get; set; }
        public string Emplacement_ { get; set; }
        public string Race_ { get; set; }
        public string Discipline_ { get; set; }
        public string NomUsager_ { get; set; }
        public string[,] tab_ { get; set; }


        public unCheval(int id, string nom, string description, string emplacement, string race, string discipline, string nomusager, string[,] tab)
        {
            ID_ = id;
            Nom_ = nom;
            Description_ = description;
            Emplacement_ = emplacement;
            Race_ = race;
            Discipline_ = discipline;
            NomUsager_ = nomusager;
            tab_ = tab;
        }
    }
}