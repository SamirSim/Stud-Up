using System;
using System.Data;
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
using System.Windows.Forms;
using System.Web;
using BespokeFusion;


namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Page
    {
        private String picName;//Lien de l'image insérée
        private String theme;//Thème choisi
        public static DayOfWeek firstDay;
        public static string firstHour;
        public static string lastHour;
        public Setting()
        {
            try
            {
                InitializeComponent();
                if (PageDAcceuil.theme == "Clair")
                {
                    radioButton.IsChecked = true;
                }
                else
                {
                    radioButton_Copy.IsChecked = true;
                }
                String chaine = comboBox_Copy.Text;
                if (firstDay.ToString() == "Sunday") chaine = "Dimanche";
                if (firstDay.ToString() == "Monday") chaine = "Lundi";
                if (firstDay.ToString() == "Tuesday") chaine = "Mardi";
                if (firstDay.ToString() == "Wednesday") chaine = "Mercredi";
                if (firstDay.ToString() == "Thursday") chaine = "Jeudi";
                if (firstDay.ToString() == "Friday") chaine = "Vendredi";
                if (firstDay.ToString() == "Satuday") chaine = "Samedi";
                comboBox_Copy1.Text = firstHour;
                comboBox_Copy2.Text = lastHour;
                comboBox_Copy.Text = chaine;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void Aide(object sender, RoutedEventArgs e)//Génerer la page d'aide en ligne
        {

        }

        private void APropos(object sender, RoutedEventArgs e)//Génere la fenêtre à propos de l'application
        {

        }

        private void Parcourir(object sender, RoutedEventArgs e)//Choisir l'image d'arrière plan
        {
            DialogResult result;
            try
            {
                using (OpenFileDialog filechooser = new OpenFileDialog())
                {
                    result = filechooser.ShowDialog();
                    this.picName = filechooser.FileName;//Chemin de l'image choisie
                }
                if (result == DialogResult.OK)
                {
                    if ((this.picName == String.Empty) || (this.picName == null))
                    {
                        System.Windows.Forms.MessageBox.Show("L'emplacement n'est pas valide ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void Appliquer(object sender, RoutedEventArgs e)//Appliquer les changement de paramètres
        {
            try
            {
                PageDAcceuil page = MainWindow.window;
                String sourceMahApps = "", sourceMaterial = "";
                ResourceDictionary newMahAppsResourceDictionary, newMaterialResourceDictionary;
                BL.CLS_Parametres parametres = new BL.CLS_Parametres();
                try
                {
                    if ((this.picName != String.Empty) && (this.picName != null))//Si l'image a bien été choisie
                    {
                        Uri imageUri = new Uri(picName, UriKind.Relative);
                        BitmapImage imageBitmap = new BitmapImage(imageUri);
                        ImageBrush image = new ImageBrush();
                        image.ImageSource = imageBitmap;
                        page.ChangeBackground(image);
                    }
                    if ((Boolean)this.radioButton.IsChecked)//Si le bouton radio "Clair" a été choisi
                    {
                        sourceMaterial = $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml";
                        sourceMahApps = $"pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml";
                        newMahAppsResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMahApps) };
                        newMaterialResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMaterial) };
                        System.Windows.Application.Current.Resources.MergedDictionaries.Remove(newMahAppsResourceDictionary);
                        System.Windows.Application.Current.Resources.MergedDictionaries.Remove(newMaterialResourceDictionary);

                        sourceMaterial = $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml";
                        sourceMahApps = $"pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml";
                        newMahAppsResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMahApps) };
                        newMaterialResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMaterial) };
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(newMahAppsResourceDictionary);
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(newMaterialResourceDictionary);
                    }
                    else
                    {
                        sourceMaterial = $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml";
                        sourceMahApps = $"pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml";
                        newMahAppsResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMahApps) };
                        newMaterialResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMaterial) };
                        System.Windows.Application.Current.Resources.MergedDictionaries.Remove(newMahAppsResourceDictionary);
                        System.Windows.Application.Current.Resources.MergedDictionaries.Remove(newMaterialResourceDictionary);

                        sourceMaterial = $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml";
                        sourceMahApps = $"pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml";
                        newMahAppsResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMahApps) };
                        newMaterialResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMaterial) };
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(newMahAppsResourceDictionary);
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add(newMaterialResourceDictionary);
                    }
                    String jour = comboBox_Copy.Text, heureD = comboBox_Copy1.Text, heureF = comboBox_Copy2.Text;
                    parametres.UpdateParametres(MainWindow.idUser, picName, this.theme, jour, heureD, heureF);//On met à jour les paramètres de l'utilisateur dans la BDD
                    firstHour = heureD;
                    lastHour = heureF;
                    page.ChangerTheme(this.theme);//On change de thème s'il a été changé
                }
                catch (Exception ex)//Une exception sera générée si aucune image n'a été choisie
                {
                    DataTable dt = parametres.SelectParametres(MainWindow.idUser);
                    //On récupère le lien de l'image d'arrière plan dans la BDD//
                    DataRow dr = dt.Rows[0];
                    int indexImage = dr.Table.Columns.IndexOf("ImageLink");
                    String imageLink = (String)dr[indexImage];
                    if ((Boolean)this.radioButton.IsChecked)//Si le bouton radio "Clair" a été choisi
                    {
                        this.theme = "Clair";
                        PageDAcceuil.theme = this.theme;
                    }
                    else if ((Boolean)this.radioButton_Copy.IsChecked)//Si le bouton radio "Sombre" a été choisi
                    {
                        this.theme = "Sombre";
                        PageDAcceuil.theme = this.theme;
                    }
                    else //Aucun bouton radio n'a été choisi
                    {
                        this.theme = PageDAcceuil.theme;
                    }
                    String jour = comboBox_Copy.Text, heureD = comboBox_Copy1.Text, heureF = comboBox_Copy2.Text;
                    parametres.UpdateParametres(MainWindow.idUser, imageLink, this.theme, jour, heureD, heureF);//On met à jour les paramètres de l'utilisateur dans la BDD
                    firstHour = heureD;
                    lastHour = heureF;
                    page.ChangerTheme(this.theme);//On change le thème
                }
                finally
                {
                    String chaine = comboBox_Copy.Text;
                    if (chaine == "Dimanche") firstDay = DayOfWeek.Sunday;
                    if (chaine == "Lundi") firstDay = DayOfWeek.Monday;
                    if (chaine == "Mardi") firstDay = DayOfWeek.Tuesday;
                    if (chaine == "Mercredi") firstDay = DayOfWeek.Wednesday;
                    if (chaine == "Jeudi") firstDay = DayOfWeek.Thursday;
                    if (chaine == "Vendredi") firstDay = DayOfWeek.Friday;
                    if (chaine == "Samedi") firstDay = DayOfWeek.Saturday;
                    page.Show();
                }

                try
                {
                    sourceMaterial = $"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor." + ((ComboBoxItem)colorTheme.SelectedItem).Tag.ToString() + ".xaml";
                    sourceMahApps = $"pack://application:,,,/MahApps.Metro;component/Styles/Accents/" + ((ComboBoxItem)colorTheme.SelectedItem).Tag.ToString() + ".xaml";
                    newMaterialResourceDictionary = new ResourceDictionary { Source = new Uri(sourceMaterial) };
                    System.Windows.Application.Current.Resources.MergedDictionaries.Add(newMaterialResourceDictionary);
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        public void RetirerImage(object sender, RoutedEventArgs e)//Retirer l'image d'arrière plan
        {
            try
            {
                String image = " ";//On affecte un chemin vide au champ image
                BL.CLS_Parametres parametres = new BL.CLS_Parametres();
                DataTable dt = parametres.SelectParametres(MainWindow.idUser);
                DataRow dr = dt.Rows[0];
                //On récupère le thème de l'utilisateur//
                int indexTheme = dr.Table.Columns.IndexOf("Theme");
                String theme = (String)dr[indexTheme];
                PageDAcceuil page = MainWindow.window;
                this.theme = theme;
                String jour = comboBox_Copy.Text, heure = comboBox_Copy1.Text;
                parametres.UpdateParametres(MainWindow.idUser, image, this.theme, jour, heure, heure);//On met à jour les paramètres de l'utilisateur dans la BDD
                this.picName = null;//Ce qui signifie qu'aucune image n'est choisie dorénavant (jusqu'à autre choix du user)
                page.ChangerMenu();//On change le menu (retire l'image)
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

    }
}
