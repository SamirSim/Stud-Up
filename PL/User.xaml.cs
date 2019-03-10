using System;
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

namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Page
    {
        public User()
        {
            InitializeComponent();
            BL.CLS_User rech = new BL.CLS_User();
            DataTable dt = rech.SelectUserId(MainWindow.idUser);//On effectue la recherche des données entrées dans la BDD
            if (dt.Rows.Count > 0)//Si la recherche donne en sortie un User, donc que les données entrées sont bonnes
            {
                string ids = dt.Rows[0]["Id"].ToString();
                int.TryParse(ids, out MainWindow.idUser);
                this.nom.Text = dt.Rows[0]["Nom"].ToString();
                this.prenom.Text = dt.Rows[0]["Prenom"].ToString();
                this.username.Text = dt.Rows[0]["UserName"].ToString();
                this.pwd.Text = dt.Rows[0]["PWD"].ToString();
            }

        }
        public void ModifierUtilisateur(object sender, RoutedEventArgs e)
        {
            Projet.BL.CLS_User md = new Projet.BL.CLS_User();
            try
            {
                md.UpdateUser(MainWindow.idUser, this.nom.Text, this.prenom.Text, this.username.Text, this.pwd.Text);
                MaterialMessageBox.Show("Modification effectuée avec succès ");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
