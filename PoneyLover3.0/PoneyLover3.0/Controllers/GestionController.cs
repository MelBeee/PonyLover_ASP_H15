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
            ViewBag.Update = "non";
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());

            if (Session["Username"].ToString() == "" && !(bool)Session["UserValid"])
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.IdDuCheval = 1;
            if (_idcheval != null)
            {
                ViewBag.Update = "oui";
                int ID = _idcheval.GetValueOrDefault();
                ViewBag.IdDuCheval = ID;
                String[] InfoCheval = Models.ClassLiaisonBD.GetInfoCheval(conn, ID);
                String[] ImageCheval = Models.ClassLiaisonBD.GetImageChevaux(conn, ID);

                ModelState.SetModelValue("TB_Nom", new ValueProviderResult(InfoCheval[1], string.Empty, new CultureInfo("en-US")));
                ViewBag.DescriptionCheval = InfoCheval[2];
                ModelState.SetModelValue("TB_Emplacement", new ValueProviderResult(InfoCheval[3], string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("TB_Race", new ValueProviderResult(InfoCheval[4], string.Empty, new CultureInfo("en-US")));
                ModelState.SetModelValue("rad1", new ValueProviderResult(InfoCheval[5], string.Empty, new CultureInfo("en-US")));
                ViewBag.Image1 = "BasePicture.png";
                ViewBag.Image2 = "BasePicture.png";
                ViewBag.Image3 = "BasePicture.png";
                if (ImageCheval.Length >= 1)
                {
                    ViewBag.Image1 = ImageCheval[0];
                }
                if (ImageCheval.Length >= 2)
                {
                    ViewBag.Image2 = ImageCheval[1];
                }
                if (ImageCheval.Length >= 3)
                {
                    ViewBag.Image3 = ImageCheval[2];
                }
            }
            else
            {
                ViewBag.Image1 = "BasePicture.png";
                ViewBag.Image2 = "BasePicture.png";
                ViewBag.Image3 = "BasePicture.png";
            }

            return View(new ImageModel());
        }

        public string RemoveSpace(string name)
        {
            name = name.Replace(" ", "");

            return name; 
        }

        [HttpPost]
        public ActionResult Gestion(String TB_Nom, String TB_Description, String TB_Emplacement, String TB_Race, String rad1, HttpPostedFileBase FileUpload1, HttpPostedFileBase FileUpload2, HttpPostedFileBase FileUpload3, String TB_Update, String TB_IDCheval)
        {
            SqlConnection conn = new SqlConnection(Session["DBPony"].ToString());

            if (TB_Nom != "" && TB_Description != "" && TB_Emplacement != "" && TB_Race != "" && rad1 != "" && Session["UserName"].ToString() != "")
            {
                if (TB_Update == "non")
                {
                    if (Models.ClassLiaisonBD.InsertionCheval(TB_Nom, TB_Description, TB_Emplacement, TB_Race, rad1, Session["UserName"].ToString(), conn))
                    {
                        ViewBag.Reussi = "Cheval ajouté !";

                        if (FileUpload1 != null && FileUpload1.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(FileUpload1.FileName);
                            string fileName = RemoveSpace(Session["UserName"].ToString() + TB_Nom + 1 + extension);
                            string path = Path.Combine(Server.MapPath("~/Images/Cheval"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            FileUpload1.SaveAs(path);
                            Models.ClassLiaisonBD.InsertionImageCheval(1, fileName, (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval")).ToString(), conn);
                        }
                        else
                        {
                            Models.ClassLiaisonBD.InsertionImageCheval(1, "BasePicture.png", (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval")).ToString(), conn);
                        }
                        if (FileUpload2 != null)
                        {
                            string extension = Path.GetExtension(FileUpload2.FileName);
                            string fileName = RemoveSpace(Session["UserName"].ToString() + TB_Nom + 2 + extension);
                            string path = Path.Combine(Server.MapPath("~/Images/Cheval"), fileName);
                            if(System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            FileUpload2.SaveAs(path);
                            Models.ClassLiaisonBD.InsertionImageCheval(2, fileName, (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval")).ToString(), conn);
                        }
                        else
                        {
                            Models.ClassLiaisonBD.InsertionImageCheval(2, "BasePicture.png", (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval")).ToString(), conn);
                        }
                        if (FileUpload3 != null)
                        {
                            string extension = Path.GetExtension(FileUpload3.FileName);
                            string fileName = RemoveSpace(Session["UserName"].ToString() + TB_Nom + 3 + extension);
                            string path = Path.Combine(Server.MapPath("~/Images/Cheval"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            FileUpload3.SaveAs(path);
                            Models.ClassLiaisonBD.InsertionImageCheval(3, fileName, (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval")).ToString(), conn);
                        }
                        else
                        {
                            Models.ClassLiaisonBD.InsertionImageCheval(3, "BasePicture.png", (Models.ClassLiaisonBD.TrouverDernierID(conn, "Cheval")).ToString(), conn);
                        }

                        ModelState.SetModelValue("TB_Nom", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                        ModelState.SetModelValue("TB_Description", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                        ModelState.SetModelValue("TB_Emplacement", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                        ModelState.SetModelValue("TB_Race", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                        ModelState.SetModelValue("rad1", new ValueProviderResult("", string.Empty, new CultureInfo("en-US")));
                        ViewBag.Image1 = "BasePicture.png";
                        ViewBag.Image2 = "BasePicture.png";
                        ViewBag.Image3 = "BasePicture.png";
                    }
                    else
                    {
                        ViewBag.NonReussi = "Erreur dans l'enregistrement du cheval !";
                    }
                }
                else
                {
                    int idcheval = int.Parse(TB_IDCheval);
					String [] tabImageCheval = Models.ClassLiaisonBD.GetImageChevaux(conn, idcheval);
                    if (Models.ClassLiaisonBD.UpdateCheval(idcheval, TB_Nom, TB_Description, TB_Emplacement, TB_Race, rad1, Session["UserName"].ToString(), conn))
                    {
                        ViewBag.Reussi = "Cheval Modifié !";

                        if (FileUpload1 != null)
                        {
                            string extension = Path.GetExtension(FileUpload1.FileName);
                            string fileName = RemoveSpace(Session["UserName"].ToString() + TB_Nom + 1 + extension);
                            string path = Path.Combine(Server.MapPath("~/Images/Cheval"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            FileUpload1.SaveAs(path);
                            ViewBag.Image1 = fileName;
                            Models.ClassLiaisonBD.UpdateImageCheval(fileName, int.Parse(TB_IDCheval), conn, 1);
                        }
                        else
 					    {
							  ViewBag.Image1 = tabImageCheval[0];

						}
                        if (FileUpload2 != null)
                        {
                            string extension = Path.GetExtension(FileUpload2.FileName);
                            string fileName = RemoveSpace(Session["UserName"].ToString() + TB_Nom + 2 + extension);
                            string path = Path.Combine(Server.MapPath("~/Images/Cheval"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            FileUpload2.SaveAs(path);
                            ViewBag.Image2 = fileName;
                            Models.ClassLiaisonBD.UpdateImageCheval(fileName, int.Parse(TB_IDCheval), conn, 2);
                        }
						else
						{
							ViewBag.Image2 = tabImageCheval[1];
						}

                        if (FileUpload3 != null)
                        {
                            string extension = Path.GetExtension(FileUpload3.FileName);
                            string fileName = RemoveSpace(Session["UserName"].ToString() + TB_Nom + 3 + extension);
                            string path = Path.Combine(Server.MapPath("~/Images/Cheval"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            FileUpload3.SaveAs(path);
                            ViewBag.Image3 = fileName;
                            Models.ClassLiaisonBD.UpdateImageCheval(fileName, int.Parse(TB_IDCheval), conn, 3);
                        }
						else
						{
							ViewBag.Image3 = tabImageCheval[2];
						}
                    }
                    else
                    {
                        ViewBag.NonReussi = "Erreur dans la modifcation du cheval !";
                    }
                }
            }
            else
            {
                ViewBag.ErreurVide = "Tout les champs doivent être remplis et au moins une photo doit être choisi.";
            }
			
            return View(new ImageModel());
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
