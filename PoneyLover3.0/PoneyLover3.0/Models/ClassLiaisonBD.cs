using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoneyLover3._0.Models
{
    public class ClassLiaisonBD
    {
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

        public static int TrouverDernierID(SqlConnection conn, string table)
        {
            int id = 0 ;

            SqlCommand sql = new SqlCommand("select max(ID) from " + table + "");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if(sqlRD.Read())
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
                SqlCommand sql = new SqlCommand(" insert into cheval(id, nom, description, emplacement, race, discipline, idusager) " +
                                                " values (" + TrouverDernierID(conn, "Cheval") + "'" + Nom + "', '" + Description + "', '" + Emplacement + "', '" + Race + "', '" + Discipline + "', " + ID + ")");
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

            SqlCommand sql = new SqlCommand("select Id from usager where NomUsager = '" + NomUsager + "')");
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

            SqlCommand sql = new SqlCommand("select password from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if(sqlDR.Read())
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

            SqlCommand sql = new SqlCommand("select email from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if(sqlDR.Read())
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