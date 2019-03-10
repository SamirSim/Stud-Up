using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet
{
    class DataAccessLayer
    {
        SqlConnection sqlconnection;

        // Constructeur
        public DataAccessLayer()
        {
            //Etablir la connexion entre l'objet et la BDD se trouvant dans le chemin défini
            //sqlconnection = new SqlConnection(@"Server=(LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\ESI\2CPI\2CPI\Semestre4\Projet 2CPI\Projet 4 Groupe 10\Réalisation\App Finale2\App Finale\AideMemoireFianle1\BDD\ProjetBDD.mdf; Integrated Security=True");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            sqlconnection = new SqlConnection(@"Server=(LocalDB)\MSSQLLocalDB; AttachDbFilename=" + path + @"\ProjetBDD.mdf; Integrated Security=True");
        }

        // Methode pour se connecter à la BDD (Ouvrir la connexion établie précédemment entre l'objet et la BDD)
        public void Open()
        {
            if (sqlconnection.State == ConnectionState.Closed)//Si la connexion est fermée
            {
                sqlconnection.Open();
            }
        }

        // Methode pour se déconnecter de la BDD (Fermer la connexion)
        public void Close()
        {
            if (sqlconnection.State == ConnectionState.Open)
            {
                sqlconnection.Close();
            }
        }

        // Methode pour rechercher des données dans la BDD
        public DataTable SelectData (string stored_procedure, SqlParameter[] param)
        {
            SqlCommand sqlcmd = new SqlCommand();//Objet de type Commande SQL pour gérer la BDD
            sqlcmd.CommandType = CommandType.StoredProcedure;//Lui affecter le type défini dans la BDD
            sqlcmd.CommandText = stored_procedure;//Lui affecter le nom de la procédure (passé en paramètre)
            sqlcmd.Connection = sqlconnection;//Lui affecter la connexion établie avec la BDD
            if (param != null)
            {
                sqlcmd.Parameters.AddRange(param);//Affecter le paramètre en entrée à la la commande définie (sqlcmd)
            }
            SqlDataAdapter da = new SqlDataAdapter (sqlcmd);//Exécuter la commande définie et la rendre en sortie
            DataTable dt = new DataTable();//On alloue une table
            try
            {
                da.Fill(dt);//Affecter la résultat de l'exécution (se trouvant dans da) au tableau dt
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        // Methode pour ajouter, modifier ou supprimer des données dans la BDD
        public void Executecommand (string stored_procedure, SqlParameter[] param)
        {
            SqlCommand sqlcmd = new SqlCommand();//Objet contenant la commande
            sqlcmd.CommandType = CommandType.StoredProcedure;//Lui affecter le type défini dans la BDD
            sqlcmd.CommandText = stored_procedure;//Lui affecter le nom de la procédure (passé en paramètre)
            sqlcmd.Connection = sqlconnection;//Lui affecter la connexion établie avec la BDD
            if (param != null)
            {
                sqlcmd.Parameters.AddRange(param);//Affecter le paramètre en entrée à la la commande définie (sqlcmd)
            }
            sqlcmd.ExecuteNonQuery();//Exécuter la commande sans avoir rien en retour (BDD mise à jour uniquement)
        }
    }
}
