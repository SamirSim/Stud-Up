﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet.BL
{
    class CLS_Document
    {
        // Méthode pour rechercher un document dans la BDD
        public DataTable SelectDocument(int DocumentId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = DocumentId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectDocument", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
        public DataTable RechDocument(int TacheId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@TacheId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = TacheId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_RechDocument", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
        // Méthode pour insérer un document dans la BDD
        public void InsertDocument(string Titre, string Emplacement, int TacheId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[3];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Titre", SqlDbType.NVarChar, 50);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Titre;
            param[1] = new SqlParameter("@Emplacement", SqlDbType.NVarChar, 50);
            param[1].Value = Emplacement;
            param[2] = new SqlParameter("@TacheId", SqlDbType.Int);
            param[2].Value = TacheId;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertDocument", param);//On exécute la procédure d'insertion implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour modifier un document dans la BDD
        public void UpdateDocument(int Id, string Titre, string Emplacement)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[3];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            param[1] = new SqlParameter("@Titre", SqlDbType.NVarChar, 50);
            param[1].Value = Titre;
            param[2] = new SqlParameter("@Emplacement", SqlDbType.NVarChar, 50);
            param[2].Value = Emplacement;
            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_UpdateDocument", param);//On exécute la procédure de modification implémentée dans la BDD
            DAL.Close();
        }

        // Methode pour supprimer un document de la BDD
        public void DeleteDocument(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteDocument", param);//On exécute la procédure de suppression implémentée dans la BDD
            DAL.Close();
        }
    }
}
