using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet.BL
{
    class CLS_Evenement
    {
        // Méthode pour rechercher un évènement dans la BDD
        public DataTable SelectEvenement(int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectEvenement", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        // Méthode pour insérer un évènement dans la BDD
        public void InsertEvenement(string Designation, DateTime DateHeure, string Lieu, int UserId, DateTime DateFin, string Commentaire, int ALerte)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[7];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Designation;
            param[1] = new SqlParameter("@DateHeure", SqlDbType.DateTime);
            param[1].Value = DateHeure;
            param[2] = new SqlParameter("@Lieu", SqlDbType.NVarChar, 50);
            param[2].Value = Lieu;
            param[3] = new SqlParameter("@UserId", SqlDbType.Int);
            param[3].Value = UserId;
            param[4] = new SqlParameter("@DateFin", SqlDbType.DateTime);
            param[4].Value = DateFin;
            param[5] = new SqlParameter("@Commentaire", SqlDbType.NVarChar, 100);
            param[5].Value = Commentaire;
            param[6] = new SqlParameter("@Alerte", SqlDbType.Int);
            param[6].Value = ALerte;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertEvenement", param);//On exécute la procédure d'insertion implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour modifier un évènement dans la BDD
        public void UpdateEvenement(int Id, string Designation, DateTime DateHeure, string Lieu, DateTime DateFin, string Commentaire, int Alerte)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[7];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            param[1] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);
            param[1].Value = Designation;
            param[2] = new SqlParameter("@DateHeure", SqlDbType.DateTime);
            param[2].Value = DateHeure;
            param[3] = new SqlParameter("@Lieu", SqlDbType.NVarChar, 50);
            param[3].Value = Lieu;
            param[4] = new SqlParameter("@DateFin", SqlDbType.DateTime);
            param[4].Value = DateFin;
            param[5] = new SqlParameter("@Commentaire", SqlDbType.NVarChar, 100);
            param[5].Value = Commentaire;
            param[6] = new SqlParameter("@Alerte", SqlDbType.Int);
            param[6].Value = Alerte;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_UpdateEvenement", param);//On exécute la procédure de modification implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour supprimer un évènement de la BDD
        public void DeleteEvenement(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteEvenement", param);//On exécute la procédure de suppression implémentée dans la BDD
            DAL.Close();
        }

        public DataTable SimulEvent(DateTime DateDebut, DateTime DateFin, int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[3];//Tableau contenant les attributs
            param[0] = new SqlParameter("@DateDebut", SqlDbType.DateTime);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = DateDebut;
            param[1] = new SqlParameter("@DateFin", SqlDbType.DateTime);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[1].Value = DateFin;
            param[2] = new SqlParameter("UserId" , SqlDbType.Int);
            param[2].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SimulEvent", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
    }
}