using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoneyLover3._0.Models
{
   public class ImageModel
   {
      public int IDImage { get; set; }

      public ImageModel()
      {
         IDImage = 1;
         Refresh();
      }

      public void Refresh()
      {
         IDImage = GetAleatoire();
      }

      public int GetAleatoire()
      {
         Random rdn = new Random();
         int math = rdn.Next(1, 10);


         return math;
      }
   }
}