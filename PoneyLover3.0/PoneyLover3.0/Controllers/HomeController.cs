using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        [HttpPost]
        public ActionResult Index(string TB_Username, string TB_Password, string TB_UsernameRemind, string btn_login, string btn_send)
        {
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());
            if(!string.IsNullOrWhiteSpace(btn_login))
            {
                bool connexionreussite = true;
                if (TB_Username == "" || TB_Password == "")
                {
                    ViewBag.ErrorTBVide = "Tout les champs doivent être remplis";
                    connexionreussite = false;
                }
                if (!Models.ClassLiaisonBD.VerifierConnexion(TB_Username, TB_Password, conn))
                {
                    ViewBag.ErrorConnexion = "Informations invalides. ";
                    connexionreussite = false;
                }

                if (connexionreussite)
                {
                    Session["UserValid"] = true;
                    Session["Username"] = TB_Username;
                }
            }
            else if (!string.IsNullOrWhiteSpace(btn_send))
            {
                bool envoitreussi = true;

                if (TB_UsernameRemind == "")
                {
                    envoitreussi = false;
                    ViewBag.ErreurVide = "Tout les champs doivent être remplis";
                }
                if (!Models.ClassLiaisonBD.NomUsagerExiste(TB_UsernameRemind, conn))
                {
                    envoitreussi = false;
                    ViewBag.ErreurUsername = "Informations invalides.";
                }

                if (envoitreussi)
                {
                    ViewBag.Reussi = "Un message a été envoyé à votre adresse courriel.";
                }
            }

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

        public ActionResult Deconnection()
        {
            Session["UserValid"] = false;
            Session["Username"] = "";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Contact(string TB_Username, string TB_Fullname, string TB_Password, string TB_PasswordConfirm, string TB_Email, string TB_EmailConfirm)
        {
            bool inscriptionreussite = true;
            bool TextBoxRempli = false;
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());
            if (TB_Password != TB_PasswordConfirm)
            {
                ViewBag.ErrorMDP = "Les mots de passe doivent correspondre";
                inscriptionreussite = false;
            }
            if (TB_Email != TB_EmailConfirm)
            {
                ViewBag.ErrorEmail = "Les emails doivent correspondre";
                inscriptionreussite = false;
            }
            if (Models.ClassLiaisonBD.NomUsagerExiste(TB_Username, conn))
            {
                ViewBag.NomUtilise = "Nom d'usager deja utilisé";
                inscriptionreussite = false;
            }
            if (TB_Username != "" && TB_Fullname != "" && TB_Email != "" && TB_EmailConfirm != "" && TB_PasswordConfirm != "" && TB_Password != "")
            {
                TextBoxRempli = true;
            }
            else
            {
                ViewBag.ErrorTBVide = "Tout les champs doivent être remplis";
                inscriptionreussite = false;
            }

            if (inscriptionreussite && TextBoxRempli)
            {
                if(Models.ClassLiaisonBD.InsertionUtilisateur(TB_Username, TB_Fullname, TB_Password, TB_Email, conn))
                {
                    ViewBag.Reussi = "Inscription Réussite !";
                }
                else
                {
                    ViewBag.NonReussi = "Erreur dans l'inscription !";
                }
            }
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