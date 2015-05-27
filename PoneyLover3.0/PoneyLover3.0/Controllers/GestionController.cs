using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoneyLover3._0.Models;
using PoneyLover3._0.Class;


namespace PoneyLover3._0.Controllers
{
    public class GestionController : Controller
    {
        public ActionResult Gestion(int? _idcheval)
        {
            if (Session["Username"].ToString() == "" && !(bool)Session["UserValid"])
            {
                return RedirectToAction("Index", "Home");
            }

            if(_idcheval != null)
            {
                int ID = _idcheval.GetValueOrDefault();
                ViewBag.NomDeCheval = ID;

                
            }

            return View(new ImageModel());
        }

        [HttpPost]
        public ActionResult Gestion(String TB_Nom, String TB_Description, String TB_Emplacement, String TB_Race, String rad1, HttpPostedFileBase FileUpload1, HttpPostedFileBase FileUpload2, HttpPostedFileBase FileUpload3)
        {
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());
            if (TB_Nom != "" && TB_Description != "" && TB_Emplacement != "" && TB_Race != "" && rad1 != "" && Session["UserName"].ToString() != "")
            {

                if (Models.ClassLiaisonBD.InsertionCheval(TB_Nom, TB_Description, TB_Emplacement, TB_Race, rad1, Session["UserName"].ToString(), conn))
                {
                    ViewBag.Reussi = "Cheval enregistrer !";

                    if (FileUpload1.FileName != "")
                        Models.ClassLiaisonBD.InsertionImageCheval(FileUpload1.FileName, (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval") - 1).ToString(), conn);
                    if (FileUpload2.FileName != "")
                        Models.ClassLiaisonBD.InsertionImageCheval(FileUpload2.FileName, (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval") - 1).ToString(), conn);
                    if (FileUpload3.FileName != "")
                        Models.ClassLiaisonBD.InsertionImageCheval(FileUpload3.FileName, (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval") - 1).ToString(), conn);


                    ModelState.SetModelValue("TB_Nom", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                    ModelState.SetModelValue("TB_Description", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                    ModelState.SetModelValue("TB_Emplacement", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                    ModelState.SetModelValue("TB_Race", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                    ModelState.SetModelValue("rad1", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
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
            return View(new ImageModel());
        }

    }
}