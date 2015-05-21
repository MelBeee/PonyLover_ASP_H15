using System;
using System.Collections.Generic;
using System.IO;
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

      public ActionResult LesChevaux()
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

      [HttpPost]
      public ActionResult Upload(HttpPostedFileBase file)
      {
         if (file != null && file.ContentLength > 0)
         {
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            file.SaveAs(path);
         }

         return RedirectToAction("UploadDocument");
      }
   }
}