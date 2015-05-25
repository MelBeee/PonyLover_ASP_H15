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
        public ActionResult Index()
        {
            return View();
        }


		public ActionResult Enregistrer(String nom, String description, String emplacement, String race, String discipline)
		{
			SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());
			if (nom != "" && description != "" && emplacement != "" && race != "" && discipline != "" && Session["UserName"].ToString() !="")
			{

				if (Models.ClassLiaisonBD.InsertionCheval(nom, description, emplacement, race, discipline, Session["UserName"].ToString(), conn))
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