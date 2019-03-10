using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet.BL
{
    class CLS_User
    {
        // Méthode pour rechercher un utilisateur dans la BDD
        public DataTable SelectUser(string UserName, string PWD)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[2];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserName;
            param[1] = new SqlParameter("@PWD", SqlDbType.NVarChar, 50);
            param[1].Value = PWD;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectUser", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
        public DataTable SelectUserId(int id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = id;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectUserId", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
        // Méthode pour insérer un utilisateur dans la BDD
        public void InsertUser(string Nom, string Prenom, string UserName, string PWD)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[4];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Nom", SqlDbType.NVarChar, 50);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Nom;
            param[1] = new SqlParameter("@Prenom", SqlDbType.NVarChar, 50);
            param[1].Value = Prenom;
            param[2] = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
            param[2].Value = UserName;
            param[3] = new SqlParameter("@PWD", SqlDbType.NVarChar, 50);
            param[3].Value = PWD;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertUser", param);//On exécute la procédure d'insertion implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour modifier un utilisateur dans la BDD
        public void UpdateUser(int Id, string Nom, string Prenom, string UserName, string PWD)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[5];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            param[1] = new SqlParameter("@Nom", SqlDbType.NVarChar, 50);
            param[1].Value = Nom;
            param[2] = new SqlParameter("@Prenom", SqlDbType.NVarChar, 50);
            param[2].Value = Prenom;
            param[3] = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);
            param[3].Value = UserName;
            param[4] = new SqlParameter("@PWD", SqlDbType.NVarChar, 50);
            param[4].Value = PWD;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_UpdateUser", param);//On exécute la procédure de modification implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour supprimer un utilisateur de la BDD
        public void DeleteUser(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteUser", param);//On exécute la procédure de suppression implémentée dans la BDD
            DAL.Close();
        }

        //Méthode qui récupère la table dans la BDD ayant le nom d'utilisateur UserName
        public DataTable SelectUserName(string UserName)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserName", SqlDbType.NVarChar, 50);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserName;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectUserName", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
    }
}
