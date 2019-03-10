using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet.BL
{
    class CLS_Parametres
    {
        // Méthode pour rechercher une activité dans la BDD (retourne une table)
        public DataTable SelectParametres(int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectParametres", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        // Méthode pour insérer une activité dans la BDD
        public void InsertParametres(string ImageLink, string Theme, int UserId,string PremierJour,string HeureDebut, string HeureFin)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[6];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@ImageLink", SqlDbType.NVarChar, 150);//Pour convertir les paramètres en types adéquats
            param[0].Value = ImageLink;
            param[1] = new SqlParameter("@Theme", SqlDbType.NVarChar, 50);
            param[1].Value = Theme;
            param[2] = new SqlParameter("@UserId", SqlDbType.Int);
            param[2].Value = UserId;
            param[3] = new SqlParameter("@PremierJour", SqlDbType.NVarChar, 50);
            param[3].Value = PremierJour;
            param[4] = new SqlParameter("@HeureDebut", SqlDbType.NVarChar, 50);
            param[4].Value = HeureDebut;
            param[5] = new SqlParameter("@HeureFin", SqlDbType.NVarChar, 50);
            param[5].Value = HeureFin;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertParametres", param);//On exécute la procédure d'insertion implémentée en BDD
            DAL.Close();
        }

        // Méthode pour modifier une activité dans la BDD
        public void UpdateParametres(int UserId, string ImageLink, string Theme, string PremierJour, string HeureDebut, string HeureFin)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[6];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserId;
            param[1] = new SqlParameter("@ImageLink", SqlDbType.NVarChar, 150);
            param[1].Value = ImageLink;
            param[2] = new SqlParameter("@Theme", SqlDbType.NVarChar, 50);
            param[2].Value = Theme;
            param[3] = new SqlParameter("@PremierJour", SqlDbType.NVarChar, 50);
            param[3].Value = PremierJour;
            param[4] = new SqlParameter("@HeureDebut", SqlDbType.NVarChar, 50);
            param[4].Value = HeureDebut;
            param[5] = new SqlParameter("@HeureFin", SqlDbType.NVarChar, 50);
            param[5].Value = HeureFin;
            DAL.Open();
            DAL.Executecommand("SP_UpdateParametres", param);//On exécute la commande de modification implémentée en BDD
            DAL.Close();
        }

        // Méthode pour supprimer une activité de la BDD
        public void DeleteParametres(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteParametres", param);//On exécute la commande de suppression implémentée en BDD
            DAL.Close();
        }
    }
}
