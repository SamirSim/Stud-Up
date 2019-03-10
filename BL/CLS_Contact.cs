using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Projet.BL
{
    class CLS_Contact
    {
        // Méthode pour rechercher un contact dans la BDD
        public DataTable SelectContact(int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectContact", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        // Méthode pour insérer un contact dans la BDD
        public void InsertContact(string Nom, string Adresse, string NumTel, string Mail, string SiteWeb, string ImageLink, int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[7];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Nom", SqlDbType.NVarChar, 50);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Nom;
            param[1] = new SqlParameter("@Adresse", SqlDbType.NVarChar, 50);
            param[1].Value = Adresse;
            param[2] = new SqlParameter("@NumTel", SqlDbType.NVarChar, 50);
            param[2].Value = NumTel;
            param[3] = new SqlParameter("@Mail", SqlDbType.NVarChar, 50);
            param[3].Value = Mail;
            param[4] = new SqlParameter("@SiteWeb", SqlDbType.NVarChar, 50);
            param[4].Value = SiteWeb;
            param[5] = new SqlParameter("@ImageLink", SqlDbType.NVarChar, 150);
            param[5].Value = ImageLink;
            param[6] = new SqlParameter("@UserId", SqlDbType.Int);
            param[6].Value = UserId;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertContact", param);//On exécute la procédure d'insertion implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour modifier un contact dans la BDD
        public void UpdateContact(int Id, string Nom, string Adresse, string NumTel, string Mail, string SiteWeb, string ImageLink)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[7];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            param[1] = new SqlParameter("@Nom", SqlDbType.NVarChar, 50);
            param[1].Value = Nom;
            param[2] = new SqlParameter("@Adresse", SqlDbType.NVarChar, 50);
            param[2].Value = Adresse;
            param[3] = new SqlParameter("@NumTel", SqlDbType.NVarChar, 50);
            param[3].Value = NumTel;
            param[4] = new SqlParameter("@Mail", SqlDbType.NVarChar, 50);
            param[4].Value = Mail;
            param[5] = new SqlParameter("@SiteWeb", SqlDbType.NVarChar, 50);
            param[5].Value = SiteWeb;
            param[6] = new SqlParameter("@ImageLink", SqlDbType.NVarChar, 150);
            param[6].Value = ImageLink;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_UpdateContact", param);//On exécute la procédure de modification implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour supprimer un contact de la BDD
        public void DeleteContact(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteContact", param);//On exécute la procédure de suppression implémentée dans la BDD
            DAL.Close();
        }
        public void DeleteAllContact()
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            DAL.Open();
            SqlParameter[] param = null;
            DAL.Executecommand("SP_DeleteAllContact", param);//On exécute la procédure de suppression implémentée dans la BDD
            DAL.Close();
        }
    }
}
