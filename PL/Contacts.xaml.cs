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
using System.IO;
using Microsoft.Win32;
using BespokeFusion;

namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for Contacts.xaml
    /// </summary>
    public partial class Contacts : Page
    {
        String chemin = "pack://application:,,,/AideMemoireFianle1;Component/PL/userImg.png";
        DataTable dt;
        int idUser = MainWindow.idUser;
        int idcontact;
        BL.CLS_Contact ListContact;
        public Contacts()
        {
            InitializeComponent();
            ListContact = new BL.CLS_Contact();
            dt = ListContact.SelectContact(idUser);
            foreach (DataRow row in dt.Rows)
            {
                if (row["ImageLink"] == null)
                {
                    row["ImageLink"] = "";
                };
            }
            contactgrid.ItemsSource = dt.DefaultView;
        }

        private void openaddcontact(object sender, RoutedEventArgs e)
        {
            try
            {
                chemin = "pack://application:,,,/AideMemoireFianle1;Component/PL/userImg.png";
                this.Nom.Text = "";
                this.Numero.Text = "";
                this.Adresse.Text = "";
                this.Mail.Text = "";
                this.siteWeb.Text = "";
                ImageContact.Source = new BitmapImage(new Uri("pack://application:,,,/AideMemoireFianle1;Component/PL/userImg.png"));
                this.modifierImage.Visibility = Visibility.Collapsed;
                this.Modifierbouton.Visibility = Visibility.Collapsed;
                this.ajouterImage.Visibility = Visibility.Visible;
                this.Ajouter.Visibility = Visibility.Visible;
                this.cancel.Visibility = Visibility.Visible;
                ajoutContactWindow.Visibility = Visibility.Visible;
                this.cancel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView c = (DataRowView)contactgrid.SelectedItem;
                int id = (int)(c.Row[0]);
                switch (MessageBox.Show("Voulez vous vraiment supprimer ce contact ?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                {
                    case (MessageBoxResult.Yes):
                        ListContact.DeleteContact(id);
                        int i = 0;
                        DataRow dr;
                        bool deleted = false;
                        while ((i < dt.Rows.Count) && (deleted == false))
                        {
                            dr = dt.Rows[i];
                            if (dr.RowState != DataRowState.Deleted)
                            {
                                if ((int)dr["Id"] == id)
                                {
                                    dr.Delete();
                                    CollectionViewSource.GetDefaultView(contactgrid.ItemsSource).Refresh();
                                    deleted = true;
                                    MaterialMessageBox.Show("Le contact est supprimé avec succès !");
                                }
                                else
                                {
                                    i++;
                                };
                            }
                            else
                            {
                                i++;
                            };
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView c = (DataRowView)contactgrid.SelectedItem;
                idcontact = (int)(c.Row[0]);
                dt.DefaultView.Sort = "Id ASC";
                int indice = dt.DefaultView.Find(idcontact.ToString());
                this.Nom.Text = (dt.DefaultView[indice]["Nom"]).ToString();
                this.Numero.Text = (dt.DefaultView[indice]["NumTel"]).ToString();
                this.Adresse.Text = (dt.DefaultView[indice]["Adresse"]).ToString();
                this.Mail.Text = (dt.DefaultView[indice]["Mail"]).ToString();
                this.siteWeb.Text = (dt.DefaultView[indice]["SiteWeb"]).ToString();
                ImageContact.Source = new BitmapImage(new Uri((dt.DefaultView[indice]["ImageLink"]).ToString(), UriKind.Absolute));
                chemin = (dt.DefaultView[indice]["ImageLink"]).ToString();
                this.ajouterImage.Visibility = Visibility.Collapsed;
                this.Ajouter.Visibility = Visibility.Collapsed;
                ajoutContactWindow.Visibility = Visibility.Visible;
                this.modifierImage.Visibility = Visibility.Visible;
                this.Modifierbouton.Visibility = Visibility.Visible;
                this.cancel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {

            }
        }

        private void Modifierbouton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (MessageBox.Show("Voulez vous sauvegarder les modifications ?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                {
                    case (MessageBoxResult.Yes):

                        if (this.Nom.Text == "")
                        {
                            MaterialMessageBox.Show("Voulez vous entrer le nom de votre contact !");
                        }
                        else
                        {
                            if (this.Numero.Text == "")
                            {
                                MaterialMessageBox.Show("Vous n'avez pas donnez le numero de votre contact !");
                            }
                            else
                            {
                                ListContact.UpdateContact(idcontact, this.Nom.Text, this.Adresse.Text, this.Numero.Text, this.Mail.Text, this.siteWeb.Text, chemin);
                                dt = ListContact.SelectContact(idUser);
                                contactgrid.ItemsSource = dt.DefaultView;
                                MaterialMessageBox.Show("Votre contact est modifié avec succès !");
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Nom.Text == "")
                {
                    MaterialMessageBox.Show("Voulez vous entrer le nom de votre contact !");
                }
                else
                {
                    if (this.Numero.Text == "")
                    {
                        MaterialMessageBox.Show("Vous n'avez pas donnez le numéro de votre contact !");
                    }
                    else
                    {
                        if (dt == null)
                        {
                            ListContact.InsertContact(this.Nom.Text, this.Adresse.Text, this.Numero.Text, this.Mail.Text, this.siteWeb.Text, chemin, idUser);
                            MaterialMessageBox.Show("Votre contact est ajouté avec succès !");
                            ajoutContactWindow.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ListContact.InsertContact(this.Nom.Text, this.Adresse.Text, this.Numero.Text, this.Mail.Text, this.siteWeb.Text, chemin, idUser);
                            dt = ListContact.SelectContact(idUser);
                            contactgrid.ItemsSource = dt.DefaultView;
                            MaterialMessageBox.Show("Votre contact est ajouté avec succès !");
                            ajoutContactWindow.Visibility = Visibility.Collapsed;
                        }
                    };
                };
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }

        }

        private void ajouterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog(); //création d'une fenetre d'exploration 
            f.DefaultExt = "jpg";
            f.Filter = " Image Files|*.jpg;*.gif;*.png;*.jpeg;*.bmp";
            f.Title = ("Voulez vous choisir une Image");
            f.ShowDialog();// affichage de cette fenetre 
            chemin = f.FileName;
            try
            {
                ImageContact.Source = new BitmapImage(new Uri(chemin, UriKind.Absolute));
            }
            catch (FileNotFoundException)
            {
                chemin = "pack://application:,,,/AideMemoireFianle1;Component/PL/userImg.png";
                ImageContact.Source = new BitmapImage(new Uri(chemin));
            }
            catch (Exception)
            {
                chemin = "pack://application:,,,/AideMemoireFianle1;Component/PL/userImg.png";
                ImageContact.Source = new BitmapImage(new Uri(chemin));
            }
            finally
            {

            }

        }

        private void closeaddcontact(object sender, RoutedEventArgs e)
        {
            ajoutContactWindow.Visibility = Visibility.Collapsed;
        }
    }
}
