using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoneyLover3._0.Models;
using PoneyLover3._0.Class;

namespace PoneyLover3._0.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new ImageModel());
        }

        // fonction pour tableau (liste) cheval
        public ActionResult About()
        {
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());

            string[,] TabChevaux = Models.ClassLiaisonBD.GetListChevaux(conn);

            int nombre = TabChevaux.Length / 7;
            unCheval[] tabdeChevaux = new unCheval[nombre];

            for (int i = 0; i < nombre; i++)
            {
                string[,] tabImage = Models.ClassLiaisonBD.GetImageChevaux(conn, int.Parse(TabChevaux[i, 0]));
                tabdeChevaux[i] = new unCheval(int.Parse(TabChevaux[i, 0]), TabChevaux[i, 1], TabChevaux[i, 2], TabChevaux[i, 3], TabChevaux[i, 4], TabChevaux[i, 5], TabChevaux[i, 6], tabImage);
            }

            unCheval[] c = new unCheval[1];
            c[0] = Models.ClassLiaisonBD.GetInfoCheval(conn, Models.ClassLiaisonBD.GetPremierID(conn));
            ViewBag.Cheval = c;

            return View(new ImageModel());
        }

        [HttpPost]
        public ActionResult Index(string TB_Username, string TB_Password, string TB_UsernameRemind, string btn_login, string btn_send)
        {
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());
            if (!string.IsNullOrWhiteSpace(btn_login))
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
                    if (EnvoyerLeEmail(TB_UsernameRemind))
                    {
                        ViewBag.Reussi = "Un message a été envoyé à votre adresse courriel.";
                    }
                    else
                    {
                        ViewBag.NonReussi = "Erreur dans l'envoit du message.";
                    }
                }
            }

            return View(new ImageModel());
        }

        private bool EnvoyerLeEmail(string TB_Username)
        {
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());

            string Email = Models.ClassLiaisonBD.GetEmail(TB_Username, conn);
            string Password = Models.ClassLiaisonBD.GetPassword(TB_Username, conn);
            bool reussi = false;

            if (Email != "" && Password != "")
            {
                Email eMail = new Email();

                // Vous devez avoir un compte gmail
                eMail.From = "tp1aspemailsender@gmail.com";
                eMail.Password = "melissa1dominic";
                eMail.SenderName = "Melissa et Charie";

                eMail.Host = "smtp.gmail.com";
                eMail.HostPort = 587;
                eMail.SSLSecurity = true;

                eMail.To = Email;
                eMail.Subject = "Rappel de Mot de Passe";
                eMail.Body = "Votre mot de passe est le suivant : "
                            + Password
                            + "<br/><br/> Attention de ne pas oublier trop souvent ! <br/> "
                            + "Melissa et Charlie";

                if (eMail.Send())
                    reussi = true;
                else
                    reussi = false;
            }
            else
            {
                reussi = false;
            }
            return reussi;
        }


        public ActionResult Contact()
        {
            return View(new ImageModel());
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
                if (Models.ClassLiaisonBD.InsertionUtilisateur(TB_Username, TB_Fullname, TB_Password, TB_Email, conn))
                {
                    ViewBag.Reussi = "Inscription Réussite !";
                }
                else
                {
                    ViewBag.NonReussi = "Erreur dans l'inscription !";
                }
            }
            return View(new ImageModel());
        }

        public ActionResult Gestion()
        {
            return View(new ImageModel());
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

        public PartialViewResult ShowImage()
        {
            return PartialView("Partial1", new ImageModel());
        }

        public ActionResult Partial1()
        {
            return PartialView("Partial1", new ImageModel());
        }
    }
}