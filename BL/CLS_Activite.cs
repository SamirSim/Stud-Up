using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet.BL
{
    class CLS_Activite
    {
        // Méthode pour rechercher une activité dans la BDD (retourne une table)
        public DataTable SelectActivite(int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectActivite", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        public DataTable RechActivite(string Designation, string TypeActivite, int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[3];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);//Pour convertir les paramètres en types adéquats
            param[0].Value = Designation;
            param[1] = new SqlParameter("@TypeActivite", SqlDbType.NVarChar, 50);
            param[1].Value = TypeActivite;
            param[2] = new SqlParameter("@UserId", SqlDbType.Int);
            param[2].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectActiviteRech", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        // Méthode pour insérer une activité dans la BDD
        public void InsertActivite(string Designation, string TypeActivite, int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[3];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);//Pour convertir les paramètres en types adéquats
            param[0].Value = Designation;
            param[1] = new SqlParameter("@TypeActivite", SqlDbType.NVarChar, 50);
            param[1].Value = TypeActivite;
            param[2] = new SqlParameter("@UserId", SqlDbType.Int);
            param[2].Value = UserId;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertActivite", param);//On exécute la procédure d'insertion implémentée en BDD
            DAL.Close();
        }

        // Méthode pour modifier une activité dans la BDD
        public void UpdateActivite(int Id, string Designation, string TypeActivite)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[3];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            param[1] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);
            param[1].Value = Designation;
            param[2] = new SqlParameter("@TypeActivite", SqlDbType.NVarChar, 50);
            param[2].Value = TypeActivite;
            DAL.Open();
            DAL.Executecommand("SP_UpdateActivite", param);//On exécute la commande de modification implémentée en BDD
            DAL.Close();
        }

        // Méthode pour supprimer une activité de la BDD
        public void DeleteActivite(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteActivite", param);//On exécute la commande de suppression implémentée en BDD
            DAL.Close();
        }
    }
}
