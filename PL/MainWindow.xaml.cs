using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using BespokeFusion;
using System.Threading;

namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static PageDAcceuil window;//La page d'acceuil de l'utilisateur courant
        public static Home emploiTemps;
        public static int idUser;//L'idenfiant de l'utilisateur courant
        public static string firstHour;
        public static string lastHour;
        public static int idEmploi;
        public MainWindow()
        {
            InitializeComponent();
            var filename = @"pack://application:,,,/AideMemoireFianle1;Component/PL/background.png";
            imageBrush.ImageSource = new BitmapImage(new Uri(filename));

        }

        public void Inscription(object sender, RoutedEventArgs e)//Inscription de l'utilisateur
        {
            BL.CLS_User user = new BL.CLS_User();
            BL.CLS_Parametres parametre = new BL.CLS_Parametres();
            try
            {
                if (this.nom.Text == "") MaterialMessageBox.Show("Veuillez introduire le nom de famille ! ");
                else
                {
                    if (this.prenom.Text == "") MaterialMessageBox.Show("Veuillez introduire le prénom ! ");
                    else
                    {
                        if (this.userNameInscription.Text == "") MaterialMessageBox.Show("Veuillez introduire le nom d'utilisateur ! ");
                        else
                        {
                            if (this.pwdInscription.Password == "") MaterialMessageBox.Show("Veuillez introduire le mot de passe ! ");
                            else
                            {
                                if (!(this.pwdInscription.Password.Equals(this.pwdNewInscription.Password))) MessageBox.Show("Veuillez introduire le même mot de passe !");
                                else
                                {
                                    DataTable dt = user.SelectUserName(userNameInscription.Text);
                                    if (dt.Rows.Count == 0)//Si l'utilisateur ne figure pas dans la base de données
                                    {
                                        user.InsertUser(nom.Text, prenom.Text, userNameInscription.Text, pwdInscription.Password);//On insère celui-ci
                                        dt = user.SelectUserName(userNameInscription.Text);//On recherche pour récupérer l'identifiant généré en BDD
                                        DataRow dr = dt.Rows[0];//On récupère une seule rangée (unique)
                                        int userId = dr.Table.Columns.IndexOf("Id");//On récupère l'index de la colonne Id
                                        int var = (int)dr[userId];//On récupère l'ID
                                        idUser = var;
                                        string b = "";
                                        parametre.InsertParametres(b, b, var, "dimanche", "8:00", "22:00");//On met à jour les paramètres de l'utilisateur dans la BDD
                                        Setting.firstDay = DayOfWeek.Sunday;
                                        Setting.firstHour = "8:00";
                                        Setting.lastHour = "22:00";
                                        BL.CLS_Activite activite = new BL.CLS_Activite();
                                        activite.InsertActivite("Planning", "MonEmploiDuTemps", idUser);
                                        DataTable Activities;
                                        Activities = activite.SelectActivite(idUser);
                                        dr = Activities.Rows[(Activities.Rows.Count) - 1];
                                        idEmploi = (int)dr["Id"];
                                        goToPageDAcceuil(sender, e);
                                    }
                                    else//L'utilisateur figure déjà dans la BDD
                                    {
                                        MaterialMessageBox.Show("Nom d'utilisateur déjà existant, veuillez le changer ! ");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Connexion(object sender, RoutedEventArgs e)
        {
            BL.CLS_User user = new BL.CLS_User();
            try
            {
                if (this.userNameConnexion.Text == "") MaterialMessageBox.Show("Veuillez introduire le nom d'utilisateur ! ");
                else
                {
                    if (this.pwdConnexion.Password == "") MaterialMessageBox.Show("Veuillez introduire le mot de passe ! ");
                    else
                    {
                        Thread.Sleep(5000);
                        DataTable dt = user.SelectUser(userNameConnexion.Text, pwdConnexion.Password);//On effectue la recherche des données entrées dans la BDD
                        if (dt.Rows.Count > 0)//Si la recherche donne en sortie un User, donc que les données entrées sont bonnes
                        {
                            DataRow dr = dt.Rows[0];//On récupère une seule rangée (unique)
                            int userId = dr.Table.Columns.IndexOf("Id");//On récupère l'index de la colonne Id
                            int var = (int)dr[userId];//On récupère l'ID
                            idUser = var;//On sauvegarde celui-ci
                            BL.CLS_Parametres parametres = new BL.CLS_Parametres();
                            dt = parametres.SelectParametres(idUser);//On récupère les paramètres de l'utilisateur
                            //On récupère le lien de l'image d'arrière plan//
                            dr = dt.Rows[0];
                            int indexImage = dr.Table.Columns.IndexOf("ImageLink");
                            int firstHourIdx = dr.Table.Columns.IndexOf("HeureDebut");
                            firstHour = (String)dr[firstHourIdx];
                            String imageLink = (String)dr[indexImage];
                            int lastHourIdx = dr.Table.Columns.IndexOf("HeureFin");
                            lastHour = (String)dr[lastHourIdx];
                            Setting.firstHour = firstHour;
                            Setting.lastHour = lastHour;
                            //--------------------------------------------//
                            goToPageDAcceuil(sender, e);
                            if (File.Exists(imageLink))//Si le chemin de l'image existe 
                            {
                                Uri imageUri = new Uri(imageLink, UriKind.Relative);
                                BitmapImage imageBitmap = new BitmapImage(imageUri);
                                ImageBrush image = new ImageBrush();
                                image.ImageSource = imageBitmap;//On convertir le chemin en image
                                window.ChangeBackground(image);//On affiche l'arrière plan de l'utilisateur
                            }
                            //On récupère le thème de l'utilisateur//
                            int indexTheme = dr.Table.Columns.IndexOf("Theme");
                            String theme = (String)dr[indexTheme];
                            PageDAcceuil.theme = theme;
                            int indexDay = dr.Table.Columns.IndexOf("PremierJour");//On récupère l'index de la colonne Id
                            string firstDay = (string)dr[indexDay];//On récupère l'ID
                            idUser = var;//On sauvegarde celui-ci
                            if (firstDay == "Dimanche") Setting.firstDay = DayOfWeek.Sunday;
                            if (firstDay == "Lundi") Setting.firstDay = DayOfWeek.Monday;
                            if (firstDay == "Mardi") Setting.firstDay = DayOfWeek.Tuesday;
                            if (firstDay == "Mercredi") Setting.firstDay = DayOfWeek.Wednesday;
                            if (firstDay == "Jeudi") Setting.firstDay = DayOfWeek.Thursday;
                            if (firstDay == "Vendredi") Setting.firstDay = DayOfWeek.Friday;
                            if (firstDay == "Samedi") Setting.firstDay = DayOfWeek.Saturday;
                            BL.CLS_Activite activite = new BL.CLS_Activite();
                            DataTable activites;
                            DataRow ligneActivite;
                            activites = activite.SelectActivite(idUser);
                            Boolean arret = false; int j = 0;
                            while ((!arret) && (j < activites.Rows.Count))
                            {
                                ligneActivite = activites.Rows[j];
                                if ((String)ligneActivite["Designation"] == "Planning")
                                {
                                    idEmploi = (int)ligneActivite["Id"];
                                    arret = true;
                                }
                                else j++;
                            }
                            window.ChangerTheme(theme);//On affiche le thème de l'utilisateur
                            //------------------------------------//
                        }
                        else//La recherche n'a rien donné donc informations erronées
                        {
                            MaterialMessageBox.Show("Nom d'utilisateur ou mot de passe erroné ! ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void goToPageDAcceuil(object sender, RoutedEventArgs e)
        {
            try
            {
                window = new PageDAcceuil();
                window.Show();
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
