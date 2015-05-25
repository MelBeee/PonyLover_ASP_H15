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
      public int ID { get; set; }
      public string Nom { get; set; }
      public string Description { get; set; }
      public string Emplacement { get; set; }
      public string Race { get; set; }
      public string Discipline { get; set; }
      public string NomUsager { get; set; }
      public string[,] tab { get; set; }
   }
}