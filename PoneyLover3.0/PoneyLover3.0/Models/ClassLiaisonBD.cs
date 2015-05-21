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

         SqlCommand sql = new SqlCommand("select nomusager from usager where nomusager = " + nomuser);
         sql.Connection = conn;
         conn.Open();

         SqlDataReader sqlDR = sql.ExecuteReader();

         if (sqlDR.Read())
         {
            resultat = sqlDR.GetString(0) != "";
         }

         sqlDR.Close();
         conn.Close();

         return resultat;
      }

      public static bool InsertionUtilisateur(string nomuser, string nomcomplet, string motdepasse, string courriel, SqlConnection conn)
      {
         bool resultat = false;

         SqlCommand sql = new SqlCommand("insert into usager(NomUsager, NomComplet, MotDePasse, AdresseCourriel) values ('" + nomuser + "', '" + nomcomplet + "', '" + motdepasse + "', '" + courriel + "')");
         sql.Connection = conn;
         conn.Open();

         if (1 == sql.ExecuteNonQuery())
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
            resultat = sqlDR.GetString(0) != "";
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
            SqlCommand sql = new SqlCommand(" insert into cheval(nom, description, emplacement, race, discipline, idusager) " +
                                            " values ('" + Nom + "', '" + Description + "', '" + Emplacement + "', '" + Race + "', '" + Discipline + "', " + ID + "')");
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

         return id;
      }

      public static bool UpdateCheval(int ID, string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
      {
         bool resultat = false;
         int ID = GetIDUsager(conn, NomUsager);

         if (ID != -1)
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