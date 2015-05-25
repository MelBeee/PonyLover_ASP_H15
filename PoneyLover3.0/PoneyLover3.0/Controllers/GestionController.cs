using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoneyLover3._0.Controllers
{
    public class GestionController : Controller
    {
        public ActionResult Gestion()
        {
           int foo = 1;
            return View();
        }

      [HttpPost]
      public ActionResult Gestion(String TB_Nom, String TB_Description, String TB_Emplacement, String TB_Race, String rad1)
		{
			SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());
         if (TB_Nom != "" && TB_Description != "" && TB_Emplacement != "" && TB_Race != "" && rad1 != "" && Session["UserName"].ToString() != "")
			{

            if (Models.ClassLiaisonBD.InsertionCheval(TB_Nom, TB_Description, TB_Emplacement, TB_Race, rad1, Session["UserName"].ToString(), conn))
				{
					ViewBag.Reussi = "Cheval enregistrer !";
				}
				else
				{
					ViewBag.NonReussi = "Erreur dans l'enregistrement du cheval !";
				}
			}
			else
			{
				 ViewBag.ErreurVide = "Tout les champs doivent être remplis";
			}
			return View();
		}

    }
}