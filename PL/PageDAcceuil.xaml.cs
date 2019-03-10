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
using System.Windows.Shapes;
using BespokeFusion;


namespace Projet.PL
{

    /// <summary>
    /// Interaction logic for PageDAcceuil.xaml
    /// </summary>
    public partial class PageDAcceuil
    {

        public Boolean isOpen = false;
        public static string theme = "Clair";
        public PageDAcceuil()
        {
            InitializeComponent();
            //this.Closing += Projet.App.Window_Closing;
            _TheFrame.Source = new Uri("Home.xaml", UriKind.Relative);
            homeButton.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
        }

        #region Interface's Events

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string targetView = ((Button)sender).Tag.ToString();
            _TheFrame.Source = new Uri(targetView, UriKind.Relative);

            if (targetView == "Home.xaml") titlePage.Text = "Planning";
            if (targetView == "Planning.xaml") titlePage.Text = "Emploi du temps";
            if (targetView == "Tasks.xaml") titlePage.Text = "Tâches";
            if (targetView == "Events.xaml") titlePage.Text = "Evènements";
            if (targetView == "Contacts.xaml") titlePage.Text = "Carnet d'adresses";
            if (targetView == "User.xaml") titlePage.Text = "Utilisateur";
            if (targetView == "Setting.xaml") titlePage.Text = "Paramètres";

            foreach (Button btn in manuPanel_main.Children)
            {
                btn.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            foreach (Button btn in manuPanel_others.Children)
            {
                btn.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
            ((Button)sender).BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
        }

        private void openCloseMenu(object sender, RoutedEventArgs e)
        {
            if (isOpen == false)
            {
                menuPanel.Width = 250;
                isOpen = true;
            }
            else
            {
                menuPanel.Width = 62;
                isOpen = false;
            }
        }

        public void ChangeBackground(ImageBrush pic)//Changer l'image d'arrière plan 
        {
            try
            {
                this.menuPanel.Background = pic;
            }
            catch (System.IO.FileNotFoundException ex)
            {

            }
        }

        public void ChangerTheme(String theme)//Changer le thème
        {
            if (theme == "Sombre")
            {
                SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3a3a3a"));
                this._TheFrame.Background = color;
                /*/
                color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F1E1E"));
                this.menuPanel.Background = color;
                /*/
            }
            else
            {
                SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ecf0f1"));
                this._TheFrame.Background = color;
                /*/
                color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F1E1E"));
                this.menuPanel.Background = color;
                /*/
            }
        }

        public void ChangerMenu()//Utilisée si l'utilisateur ne veut pas avoir d'image d'arrière plan au menu
        {
            SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"));
            this.menuPanel.Background = color;
        }

        #endregion

        private void deconnectButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PL.MainWindow.window = null;
            MainWindow window = new MainWindow();
            window.Show();
        }
    }

}
