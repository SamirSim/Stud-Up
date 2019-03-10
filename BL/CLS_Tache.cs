using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet.BL
{
    class CLS_Tache
    {
        // Méthode pour rechercher une tache dans la BDD
        public DataTable SelectTache(int ActiviteId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@ActiviteId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = ActiviteId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectTache", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        // Méthode pour insérer une tâche dans la BDD
        public void InsertTache(string Designation, int Priorite, DateTime DateHeure, int Etat, int ActiviteId, DateTime DateFin, string Commentaire, int Alerte, int Util)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[9];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Designation;
            param[1] = new SqlParameter("@Priorite", SqlDbType.Int);
            param[1].Value = Priorite;
            param[2] = new SqlParameter("@DateHeure", SqlDbType.DateTime);
            param[2].Value = DateHeure;
            param[3] = new SqlParameter("@Etat", SqlDbType.Int);
            param[3].Value = Etat;
            param[4] = new SqlParameter("@ActiviteId", SqlDbType.Int);
            param[4].Value = ActiviteId;
            param[5] = new SqlParameter("@DateFin", SqlDbType.DateTime);
            param[5].Value = DateFin;
            param[6] = new SqlParameter("@Commentaire", SqlDbType.NVarChar, 100);
            param[6].Value = Commentaire;
            param[7] = new SqlParameter("@Alerte", SqlDbType.Int);
            param[7].Value = Alerte;
            param[8] = new SqlParameter("@Util", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[8].Value = Util;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertTache", param);//On exécute la procédure d'insertion implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour modifier une tâche dans la BDD
        public void UpdateTache(int Id, string Designation, int Priorite, DateTime DateHeure, int Etat, DateTime DateFin, string Commentaire, int Alerte)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[8];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            param[1] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);
            param[1].Value = Designation;
            param[2] = new SqlParameter("@Priorite", SqlDbType.Int);
            param[2].Value = Priorite;
            param[3] = new SqlParameter("@DateHeure", SqlDbType.DateTime);
            param[3].Value = DateHeure;
            param[4] = new SqlParameter("@Etat", SqlDbType.Int);
            param[4].Value = Etat;
            param[5] = new SqlParameter("@DateFin", SqlDbType.DateTime);
            param[5].Value = DateFin;
            param[6] = new SqlParameter("@Commentaire", SqlDbType.NVarChar, 100);
            param[6].Value = Commentaire;
            param[7] = new SqlParameter("@Alerte", SqlDbType.Int);
            param[7].Value = Alerte;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_UpdateTache", param);//On exécute la procédure de modification implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour supprimer une tâche de la BDD
        public void DeleteTache(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteTache", param);//On exécute la procédure de suppression implémentée dans la BDD
            DAL.Close();
        }

        // ...
        public DataTable SimulTache(DateTime DateDebut, DateTime DateFin,int Util)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[3];//Tableau contenant les attributs
            param[0] = new SqlParameter("@DateDebut", SqlDbType.DateTime);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = DateDebut;
            param[1] = new SqlParameter("@DateFin", SqlDbType.DateTime);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[1].Value = DateFin;
            param[2] = new SqlParameter("@Util", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[2].Value = Util;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SimulTache", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
    }
}
