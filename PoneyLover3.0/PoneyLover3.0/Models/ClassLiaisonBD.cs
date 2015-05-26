using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoneyLover3._0.Models
{
    public class ClassLiaisonBD
    {

        public static string GetNomUsager(SqlConnection conn, int id)
        {
            string nomusager = "";
            SqlCommand sql = new SqlCommand("select nomusager from usager where id = " + id);
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                nomusager = sqlDR.GetString(0);
            }

            conn.Close();
            sqlDR.Close();

            return nomusager;
        }

        public static string[,] GetListChevaux(SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval ");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            string[,] Tab = new string[nombre, 7];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt, 0] = sqlDR2.GetInt32(0).ToString();
                    Tab[cpt, 1] = sqlDR2.GetString(1);
                    Tab[cpt, 2] = sqlDR2.GetString(2);
                    Tab[cpt, 3] = sqlDR2.GetString(3);
                    Tab[cpt, 4] = sqlDR2.GetString(4);
                    Tab[cpt, 5] = sqlDR2.GetString(5);
                    Tab[cpt, 6] = sqlDR2.GetInt32(6).ToString();
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static string[,] GetImageChevaux(SqlConnection conn, int IDCheval)
        {
            SqlCommand sql = new SqlCommand("select id, guidphoto from photo where idcheval = " + IDCheval + "");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            sqlDR.Close();
            string[,] Tab = new string[nombre, 2];
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;

                while (sqlDR2.Read())
                {
                    Tab[cpt, 0] = sqlDR2.GetInt32(0).ToString();
                    Tab[cpt, 1] = sqlDR2.GetString(1);
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static bool NomUsagerExiste(string nomuser, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select nomusager from usager where nomusager = '" + nomuser + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string[] GetChevaux(string Username, SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select count(nom), nom from cheval where idusager = " + GetIDUsager(conn, Username) + " group by nom");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombreresultat = 0;
            if (sqlDR.Read())
            {
                nombreresultat = sqlDR.GetInt32(0);
            }
            string[] Tab = new string[nombreresultat];
            if (nombreresultat >= 1)
            {
                int cpt = 1;
                while (sqlDR.Read())
                {
                    Tab[cpt] = sqlDR.GetString(1);
                    cpt++;
                }
            }

            conn.Close();
            sqlDR.Close();

            return Tab;
        }

        public static bool InsertionImageCheval(String GuidCheval, String IDCheval, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("insert into photo(ID,GuidPhoto,IDCheval) values (" + TrouverDernierID(conn, "usager") + ",'" + GuidCheval + "','" + IDCheval + "')");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            if (ligne == 1)
            {
                resultat = true;
            }

            conn.Close();

            return resultat;
        }

        public static int TrouverDernierID(SqlConnection conn, string table)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select max(ID) from " + table + "");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0) + 1;
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static bool InsertionUtilisateur(string nomuser, string nomcomplet, string motdepasse, string courriel, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("insert into usager(ID, NomUsager, NomComplet, MotDePasse, AdresseCourriel)" +
                                            "values (" + TrouverDernierID(conn, "usager") + ",'" + nomuser + "', '" + nomcomplet + "', '" + motdepasse + "', '" + courriel + "')");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            if (ligne == 1)
            {
                resultat = true;
            }

            conn.Close();

            return resultat;
        }

        public static bool VerifierConnexion(string nomuser, string motdepasse, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select NomUsager from usager where NomUsager = '" + nomuser + "' and MotDePasse = '" + motdepasse + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static bool InsertionCheval(string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            bool resultat = false;
            int ID = GetIDUsager(conn, NomUsager);

            if (ID != -1)
            {
                SqlCommand sql = new SqlCommand("insert into cheval(id, nom, description, emplacement, race, discipline, idusager) " +
                                                " values (" + TrouverDernierID(conn, "Cheval") + ",'" + Nom + "', '" + Description + "', '" + Emplacement + "', '" + Race + "', '" + Discipline + "', " + ID + ")");
                sql.Connection = conn;
                conn.Open();

                if (1 == sql.ExecuteNonQuery())
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static int GetIDUsager(SqlConnection conn, string NomUsager)
        {
            int id = -1;

            SqlCommand sql = new SqlCommand("select Id from usager where NomUsager = '" + NomUsager + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetPassword(string Username, SqlConnection conn)
        {
            string mdp = "";

            SqlCommand sql = new SqlCommand("select motdepasse from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                mdp = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return mdp;
        }

        public static string GetEmail(string Username, SqlConnection conn)
        {
            string email = "";

            SqlCommand sql = new SqlCommand("select adressecourriel from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                email = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return email;
        }

        public static bool UpdateCheval(int ID, string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            bool resultat = false;
            int IDUsager = GetIDUsager(conn, NomUsager);

            if (IDUsager != -1)
            {
                SqlCommand sql = new SqlCommand(" update cheval set nom = '" + Nom + "', "
                                                                + " description = '" + Description + "', "
                                                                + " emplacement = '" + Emplacement + "', "
                                                                + " race = '" + Race + "', "
                                                                + " discipline = '" + Discipline + "', "
                                                                + " where ID = " + ID);
                sql.Connection = conn;
                conn.Open();

                if (1 == sql.ExecuteNonQuery())
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }
    }
}