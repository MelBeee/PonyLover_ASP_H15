﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoneyLover3._0.Controllers
{
   public class HomeController : Controller
   {
      public ActionResult Index()
      {
         return View();
      }

      public ActionResult About()
      {
         return View();
      }

      public ActionResult Contact()
      {
         return View();
      }

      public ActionResult Gestion()
      {
         return View(); 
      }
   }
}