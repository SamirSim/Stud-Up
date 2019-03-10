using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Projet.BL
{
    class CLS_Cellule
    {
        // Méthode pour rechercher une cellule de l'emploi du temps dans la BDD
        public DataTable SelectCellule(int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectCellule", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        // Méthode pour insérer une tâche dans la BDD
        public void InsertCellule(string Abrv, string Designation, string HeureDebut, string HeureFin, string Salle, int jour, int Type, string Enseignant, int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[9];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Abrv", SqlDbType.NVarChar, 5);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Abrv;
            param[1] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);
            param[1].Value = Designation;
            param[2] = new SqlParameter("@HeureDebut", SqlDbType.Time, 7);
            param[2].Value = HeureDebut;
            param[3] = new SqlParameter("@HeureFin", SqlDbType.Time, 7);
            param[3].Value = HeureFin;
            param[4] = new SqlParameter("@Salle", SqlDbType.NVarChar, 50);
            param[4].Value = Salle;
            param[5] = new SqlParameter("@jour", SqlDbType.Int);
            param[5].Value = jour;
            param[6] = new SqlParameter("@Type", SqlDbType.Int);
            param[6].Value = Type;
            param[7] = new SqlParameter("@Enseignant", SqlDbType.NVarChar, 50);
            param[7].Value = Enseignant;
            param[8] = new SqlParameter("@UserId", SqlDbType.Int);
            param[8].Value = UserId;

            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_InsertCellule", param);//On exécute la procédure d'insertion implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour modifier une cellule de l'emploi du temps dans la BDD
        public void UpdateCellule(int Id, string Abrv, string Designation, string HeureDebut, string HeureFin, string Salle, int jour, int Type, string Enseignant)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[9];//Tableau contenant les attributs
            /*/ On affecte tous les attributs au paramètre /*/
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            param[1] = new SqlParameter("@Abrv", SqlDbType.NVarChar, 5);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[1].Value = Abrv;
            param[2] = new SqlParameter("@Designation", SqlDbType.NVarChar, 50);
            param[2].Value = Designation;
            param[3] = new SqlParameter("@HeureDebut", SqlDbType.Time, 7);
            param[3].Value = HeureDebut;
            param[4] = new SqlParameter("@HeureFin", SqlDbType.Time, 7);
            param[4].Value = HeureFin;

            param[5] = new SqlParameter("@Salle", SqlDbType.NVarChar, 50);
            param[5].Value = Salle;
            param[6] = new SqlParameter("@jour", SqlDbType.Int);
            param[6].Value = jour;
            param[7] = new SqlParameter("@Type", SqlDbType.Int);
            param[7].Value = Type;
            param[8] = new SqlParameter("@Enseignant", SqlDbType.NVarChar, 50);
            param[8].Value = Enseignant;

            /*Ainsi on passe le paramètre sous forme de tableau d'attributs /*/
            DAL.Open();
            DAL.Executecommand("SP_UpdateCellule", param);//On exécute la procédure de modification implémentée dans la BDD
            DAL.Close();
        }

        // Méthode pour supprimer une Cellule de la BDD
        public void DeleteCellule(int Id)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[1];//Tableau contenant les attributs
            param[0] = new SqlParameter("@Id", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = Id;
            DAL.Open();
            DAL.Executecommand("SP_DeleteCellule", param);//On exécute la procédure de suppression implémentée dans la BDD
            DAL.Close();
        }
        public DataTable SelectCelluleTime(int UserId, int Jour, DateTime HeureDebut, DateTime HeureFin)//on vérifie s'il y a pas de chauvauchement des séances
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[4];//Tableau contenant les attributs
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = UserId;
            param[1] = new SqlParameter("@Jour", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[1].Value = Jour;
            param[2] = new SqlParameter("@HeureDebut", SqlDbType.Time, 7);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[2].Value = HeureDebut;
            param[3] = new SqlParameter("@HeureFin", SqlDbType.Time, 7);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[3].Value = HeureFin;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SelectCellule", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }

        public DataTable SimulCours(String DateDebut, String DateFin, int Jour, int UserId)
        {
            DataAccessLayer DAL = new DataAccessLayer();//Variable pour accéder à la BDD
            SqlParameter[] param = new SqlParameter[4];//Tableau contenant les attributs
            param[0] = new SqlParameter("@DateDebut", SqlDbType.Time,7);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[0].Value = DateDebut;
            param[1] = new SqlParameter("@DateFin", SqlDbType.Time,7);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[1].Value = DateFin;
            param[2] = new SqlParameter("@Jour", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[2].Value = Jour;
            param[3] = new SqlParameter("@UserId", SqlDbType.Int);//Pour transférer le paramètre en respectant le type (SQL-INJECTION)
            param[3].Value = UserId;
            DAL.Open();
            DataTable dt = new DataTable();//On alloue un espace mémoire pour la table à rendre en sortie
            dt = DAL.SelectData("SP_SimulCours", param);//On exécute la procédure de recherche implémentée dans la BDD
            DAL.Close();
            return dt;
        }
    }
}

