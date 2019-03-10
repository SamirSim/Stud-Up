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
using System.ComponentModel;
using Syncfusion.UI.Xaml.Schedule;
using System.Globalization;
using System.Threading;
using Syncfusion.Windows.Shared;
using System.IO;
using Microsoft.Win32;
using System.Media;
using System.Data;
using System.Collections.ObjectModel;
using BespokeFusion;

namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        #region Members
        internal Appointment SelectedAppointment;
        internal BindingClass AddDataContext = null;
        public AppointmentEditorOpeningEventArgs eSauve;
        ScheduleAppointmentCollection AppointmentCollection = new ScheduleAppointmentCollection();
        ObservableCollection<SolidColorBrush> brush = new ObservableCollection<SolidColorBrush>();
        DataTable emploisutemps = new DataTable();
        BL.CLS_Cellule cellule = new BL.CLS_Cellule();
        BL.CLS_Tache Taches = new BL.CLS_Tache();
        BL.CLS_Evenement Event = new BL.CLS_Evenement();
        BL.CLS_Activite Activity = new BL.CLS_Activite();
        BL.CLS_DocumentEvent documentEvent = new BL.CLS_DocumentEvent();
        int userId = MainWindow.idUser;
        DataTable Activities = new DataTable();
        DataTable taches = new DataTable();
        DataTable Events = new DataTable();
        Reminder reminder;
        SoundPlayer player = new SoundPlayer();
        BL.CLS_Document document = new BL.CLS_Document();
        int idTache;
        int IdentifiantEmploiDuTemps;
        SolidColorBrush colorTache = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
        SolidColorBrush colorEvent = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f39c12"));
        string doc;
        #endregion
        #region Constructor
        public Home()
        {
            try
            {
                InitializeComponent();
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
                CultureInfo culture;
                culture = CultureInfo.CreateSpecificCulture("fr-FR");
                Thread.CurrentThread.CurrentCulture = culture;
                customeEditorTache.DataContext = this;
                customeEditorTache.Visibility = Visibility.Collapsed;
                customeEditorEvent.Visibility = Visibility.Collapsed;
                Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
                Schedule.ReminderOpening += Schedule_ReminderOpening;
                customeEditorTache.DataContext = this;
                customeEditorTache.Visibility = Visibility.Collapsed;
                customeEditorEvent.Visibility = Visibility.Collapsed;
                Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xA2, 0xC1, 0x39)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xD8, 0x00, 0x73)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x1B, 0xA1, 0xE2)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xF0, 0x96, 0x09)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x33, 0x99, 0x33)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x00, 0xAB, 0xA9)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));
                Activities = Activity.SelectActivite(userId);
                string lienIcone;
                foreach (DataRow ligneact in this.Activities.Rows)
                {
                    taches = Taches.SelectTache((int)ligneact["Id"]);
                    if (this.taches != null)
                    {
                        foreach (DataRow ligne in this.taches.Rows)
                        {
                            int iddoc;
                            lienIcone = null;
                            DataTable docs = new DataTable();
                            BL.CLS_Document document = new BL.CLS_Document();
                            docs = document.RechDocument((int)ligne["Id"]);
                            if (docs.Rows.Count == 0) iddoc = -1;
                            else iddoc = (int)docs.Rows[0]["Id"];
                            DateTime date1 = new DateTime(int.Parse(((string)ligne["HeureDebut"]).Substring(0, 4)), int.Parse(((string)ligne["HeureDebut"]).Substring(5, 2)), int.Parse(((string)ligne["HeureDebut"]).Substring(8, 2)), int.Parse(((string)ligne["HeureDebut"]).Substring(11, 2)), int.Parse(((string)ligne["HeureDebut"]).Substring(14, 2)), 0);
                            DateTime date2 = new DateTime(int.Parse(((string)ligne["HeureFin"]).Substring(0, 4)), int.Parse(((string)ligne["HeureFin"]).Substring(5, 2)), int.Parse(((string)ligne["HeureFin"]).Substring(8, 2)), int.Parse(((string)ligne["HeureFin"]).Substring(11, 2)), int.Parse(((string)ligne["HeureFin"]).Substring(14, 2)), 0);
                            int cpt = 0;
                            int iter = 1;
                            int semaine = 7;
                            if ((string)ligneact["Designation"] == "Planning")
                            {
                                lienIcone = "pack://application:,,,/AideMemoireFianle1;Component/IconeAppointment/cours.png";
                                iter = 19;
                                if (IdentifiantEmploiDuTemps == 0)
                                {
                                    IdentifiantEmploiDuTemps = (int)ligne["ActiviteId"];
                                }
                            }
                            while (cpt < iter)
                            {
                                if ((string)ligneact["Designation"] == "Planning")
                                    AppointmentCollection.Add(new Appointment()
                                    {
                                        AppointmentImageURI = new BitmapImage(new Uri(lienIcone)),
                                        StartTime = date1,
                                        EndTime = date2,
                                        AppointmentBackground = brush[(int)ligne["ActiviteId"] % 3],
                                        Subject = (string)ligne["Designation"],
                                        selectedEtat = (int)ligne["Etat"],
                                        TypeAjout = TypeAjout.TASK,
                                        selectedPriority = (int)ligne["Priorite"],
                                        IdActivite = (int)ligne["ActiviteId"],
                                        IdTache = (int)ligne["Id"],
                                        IdCellule = (int)ligne["Etat"],
                                        Notes = (string)ligne["Commentaire"],
                                    });
                                else
                                {
                                    AppointmentCollection.Add(new Appointment()
                                    {
                                        StartTime = date1,
                                        EndTime = date2,
                                        AppointmentBackground = brush[(int)ligne["ActiviteId"] % 3],
                                        Subject = (string)ligne["Designation"],
                                        selectedEtat = (int)ligne["Etat"],
                                        TypeAjout = TypeAjout.TASK,
                                        selectedPriority = (int)ligne["Priorite"],
                                        IdActivite = (int)ligne["ActiviteId"],
                                        IdTache = (int)ligne["Id"],
                                        IdCellule = (int)ligne["Etat"],
                                        Notes = (string)ligne["Commentaire"],
                                        IdDocument = iddoc,
                                        selectedReminder = (int)ligne["Alerte"]
                                    });
                                }
                                date1 = date1.AddDays(semaine);
                                date2 = date2.AddDays(semaine);
                                cpt++;
                            }
                            Schedule.Appointments = AppointmentCollection;
                        }
                    }
                    else
                    {
                        MaterialMessageBox.Show("Aucun emploi du temps !");
                    }
                }
                Events = Event.SelectEvenement(userId);
                if (this.Events != null)
                {
                    foreach (DataRow ligne in this.Events.Rows)
                    {
                        try
                        {
                            int iddocev;
                            DataTable docev = new DataTable();
                            docev = documentEvent.RechDocument((int)ligne["Id"]);
                            if (docev.Rows.Count != 0) iddocev = (int)docev.Rows[0]["Id"];
                            else iddocev = -1;
                            DateTime date1 = new DateTime(int.Parse(((string)ligne["HeureDebut"]).Substring(0, 4)), int.Parse(((string)ligne["HeureDebut"]).Substring(5, 2)), int.Parse(((string)ligne["HeureDebut"]).Substring(8, 2)), int.Parse(((string)ligne["HeureDebut"]).Substring(11, 2)), int.Parse(((string)ligne["HeureDebut"]).Substring(14, 2)), 0);
                            DateTime date2 = new DateTime(int.Parse(((string)ligne["HeureFin"]).Substring(0, 4)), int.Parse(((string)ligne["HeureFin"]).Substring(5, 2)), int.Parse(((string)ligne["HeureFin"]).Substring(8, 2)), int.Parse(((string)ligne["HeureFin"]).Substring(11, 2)), int.Parse(((string)ligne["HeureFin"]).Substring(14, 2)), 0);

                            AppointmentCollection.Add(new Appointment()
                            {
                                AppointmentImageURI = new BitmapImage(new Uri("pack://application:,,,/AideMemoireFianle1;Component/IconeAppointment/star.png")),
                                StartTime = date1,
                                EndTime = date2,
                                AppointmentBackground = brush[6],
                                Subject = (string)ligne["Designation"],
                                TypeAjout = TypeAjout.EVENT,
                                Location = (string)ligne["Lieu"],
                                IdEvenement = (int)ligne["Id"],
                                Notes = (string)ligne["Commentaire"],
                                IdDocument = iddocev,
                                selectedReminder = (int)ligne["Alerte"]
                            });
                        }
                        catch (Exception ex)
                        {

                        }
                        Schedule.Appointments = AppointmentCollection;
                    }
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        #endregion

        #region Events

        void Schedule_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            try
            {
                Appointment app;
                app = (Appointment)(e.Appointment);
                eSauve = e;
                e.Cancel = true;
                Schedule.IsHitTestVisible = false;
                AddDataContext = new BindingClass()
                {
                    CurrentSelectedDate = e.StartTime,
                    Appointment = e.Appointment
                };
                if (e.Appointment != null)
                {
                    if (app.TypeAjout == TypeAjout.TASK)
                        editAppointmentTache();
                    else if (app.TypeAjout == TypeAjout.EVENT)
                        editAppointmentEvent();
                }
                else
                {
                    chooseAppointementType.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void annulerType(object sender, RoutedEventArgs e)
        {
            chooseAppointementType.IsOpen = false;
            Schedule.IsHitTestVisible = true;
        }

        private void goToTache(object sender, RoutedEventArgs e)
        {
            try
            {
                chooseAppointementType.IsOpen = false;
                Schedule.IsHitTestVisible = true;
                addAppointmentTache();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void goToEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                chooseAppointementType.IsOpen = false;
                Schedule.IsHitTestVisible = true;
                addAppointmentEvent();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void ajouterDocs(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                doc = openFileDialog.FileName;
                this.titleDoc.Text = doc.Split('\\').Last();
            }

        }

        #endregion

        #region EditorTache
        private void addAppointmentTache()
        {
            try
            {
                Activite.Items.Clear();
                Activite.IsEnabled = true;
                TypeActivite1.IsEnabled = true;
                ajoutActivite.IsEnabled = true;
                customeEditorTache.Visibility = Visibility.Visible;
                Schedule.IsHitTestVisible = false;
                SelectedAppointment = null;
                TypeActivite.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentActivity));
                TypeActivite1.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentActivity));
                TypeActivite.SelectedIndex = 0;
                TypeActivite1.SelectedIndex = 0;
                priorityTache.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentPriority));
                priorityTache.SelectedIndex = 0;
                etatTache.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentEtat));
                etatTache.SelectedIndex = 0;
                reminderTache.ItemsSource = Enum.GetValues(typeof(ReminderTimeType));
                reminderTache.SelectedIndex = 0;
                addstarttimeTache.DateTime = eSauve.StartTime;
                addstartmonthTache.SelectedDate = eSauve.StartTime;
                addendmonthTache.SelectedDate = eSauve.StartTime.AddHours(1);
                addendtimeTache.DateTime = eSauve.StartTime.AddHours(1);
                BL.CLS_Activite activite = new BL.CLS_Activite();
                DataTable dt = activite.SelectActivite(MainWindow.idUser);
                int i = 0;
                foreach (DataRow ligne in dt.Rows)
                {
                    if ((String)ligne["Designation"] != "Planning") Activite.Items.Add((String)ligne["Designation"]);
                    i++;
                }
                subTache.Text = "";
                notesTache.Text = "";
                titleDoc.Text = "";
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void editAppointmentTache()
        {
            try
            {
                Activite.Items.Clear();
                Activite.IsEnabled = false;
                customeEditorTache.Visibility = Visibility.Visible;
                Schedule.IsHitTestVisible = false;
                DataContext = AddDataContext;
                SelectedAppointment = AddDataContext.Appointment as Appointment;
                TypeActivite.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentActivity));
                TypeActivite.SelectedIndex = SelectedAppointment.selectedActivity;
                TypeActivite1.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentActivity));
                subTache.IsReadOnly = false;
                priorityTache.IsEnabled = true;
                addendmonthTache.Visibility = Visibility.Visible;
                addstartmonthTache.Visibility = Visibility.Visible;
                TypeActivite.Visibility = Visibility.Visible;
                addstarttimeTache.IsEnabled = true;
                addendtimeTache.IsEnabled = true;
                etatTache.IsEnabled = true;
                TypeActivite1.IsEnabled = false;
                ajoutActivite.IsEnabled = false;
                priorityTache.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentPriority));
                priorityTache.SelectedIndex = SelectedAppointment.selectedPriority;
                if (SelectedAppointment.IdActivite == MainWindow.idEmploi)
                {
                    priorityTache.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentPriority));
                    etatTache.SelectedIndex = 0;
                    etatTache.IsEnabled = false;
                    TypeActivite.IsEnabled = false;
                    subTache.IsReadOnly = true;
                    priorityTache.IsEnabled = false;
                    addendmonthTache.IsEnabled = false;
                    addstartmonthTache.IsEnabled = false;
                    addstarttimeTache.IsEnabled = false;
                    addendtimeTache.IsEnabled = false;
                }
                BL.CLS_Activite activite = new BL.CLS_Activite();
                DataTable dt = activite.SelectActivite(MainWindow.idUser);
                int i = 0;
                foreach (DataRow ligne in dt.Rows)
                {
                    if ((String)ligne["Designation"] != "Planning") Activite.Items.Add((String)ligne["Designation"]);
                    i++;
                }
                etatTache.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentEtat));
                etatTache.SelectedIndex = SelectedAppointment.selectedEtat;
                DataTable docs = new DataTable();
                BL.CLS_Document document = new BL.CLS_Document();
                docs = document.SelectDocument(SelectedAppointment.IdDocument);
                if (docs.Rows.Count != 0)
                {
                    try
                    {
                        this.titleDoc.Text = (string)docs.Rows[0]["Titre"];
                        doc = (string)docs.Rows[0]["Emplacement"];
                    }
                    catch (Exception ex)
                    { }
                }
                reminderTache.ItemsSource = Enum.GetValues(typeof(ReminderTimeType));
                reminderTache.SelectedIndex = SelectedAppointment.selectedReminder;
                subTache.Text = SelectedAppointment.Subject;
                notesTache.Text = SelectedAppointment.Notes;
                addstarttimeTache.DateTime = (AddDataContext.Appointment as Appointment).StartTime;
                addstartmonthTache.SelectedDate = (AddDataContext.Appointment as Appointment).StartTime;
                addendtimeTache.DateTime = (AddDataContext.Appointment as Appointment).EndTime;
                addendmonthTache.SelectedDate = (AddDataContext.Appointment as Appointment).EndTime;
                BL.CLS_Activite activites = new BL.CLS_Activite();
                docs = activites.SelectActivite(MainWindow.idUser);
                i = 0;
                foreach (DataRow ligne in docs.Rows)
                {
                    if ((int)ligne["Id"] == SelectedAppointment.IdActivite)
                    {
                        Activite.Text = (String)ligne["Designation"];
                        TypeActivite1.Text = (String)ligne["TypeActivite"];
                    }
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void saveTache(object sender, RoutedEventArgs e)
        {
            try
            {

                Appointment appointment;
                if (Schedule.SelectedAppointment == null)
                {
                    appointment = new Appointment();
                    appointment.TypeAjout = TypeAjout.TASK;
                }
                else
                {
                    appointment = SelectedAppointment;
                }
                DateTime date = (DateTime)addstarttimeTache.DateTime;
                DateTime date1 = (DateTime)addendtimeTache.DateTime;
                if (((DateTime)addstartmonthTache.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second)) <= ((DateTime)addendmonthTache.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second)))
                {
                    if (SelectedAppointment == null)
                    {
                        if (Taches.SimulTache(((DateTime)addstartmonthTache.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second)), ((DateTime)addendmonthTache.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second)), MainWindow.idUser).Rows.Count != 0)
                        {
                            MaterialMessageBox.Show("Une tâche est déjà programmée à ce moment !");
                        }
                        else
                        {
                            appointment.ReminderTime = (ReminderTimeType)reminderTache.SelectedItem;
                            appointment.selectedReminder = reminderTache.SelectedIndex;
                            date = (DateTime)addstarttimeTache.DateTime;
                            appointment.StartTime = ((DateTime)addstartmonthTache.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            date1 = (DateTime)addendtimeTache.DateTime;
                            appointment.EndTime = ((DateTime)addendmonthTache.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            appointment.Subject = subTache.Text;
                            appointment.Notes = notesTache.Text;
                            appointment.selectedActivity = TypeActivite.SelectedIndex;
                            appointment.selectedPriority = priorityTache.SelectedIndex;
                            appointment.selectedEtat = etatTache.SelectedIndex;
                            appointment.AppointmentBackground = colorTache;
                            DataRow dr;
                            Activities = Activity.RechActivite(Activite.Text, TypeActivite1.Text, MainWindow.idUser);
                            if (Activities.Rows.Count == 0)
                            {
                                MaterialMessageBox.Show("Le type de l'activité ne correspond pas");
                            }
                            else
                            {
                                dr = Activities.Rows[0];
                                appointment.IdActivite = (int)dr["Id"];
                                Taches.InsertTache(appointment.Subject, appointment.selectedPriority, appointment.StartTime, appointment.selectedEtat, appointment.IdActivite, appointment.EndTime, appointment.Notes, appointment.selectedReminder, MainWindow.idUser);
                                taches.Clear();
                                taches = Taches.SelectTache(appointment.IdActivite);
                                dr = taches.Rows[(taches.Rows.Count) - 1];
                                appointment.IdTache = (int)dr["Id"];
                                try
                                {
                                    document.InsertDocument(this.titleDoc.Text, doc, appointment.IdTache);

                                }
                                catch (Exception ex)
                                {

                                }
                                Schedule.Appointments.Add(appointment);
                                customeEditorTache.Visibility = Visibility.Collapsed;
                                Schedule.IsHitTestVisible = true;
                            }
                        }
                    }
                    else
                    {
                        DataTable dt = Taches.SimulTache(((DateTime)addstartmonthTache.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second)), ((DateTime)addendmonthTache.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second)), MainWindow.idUser);
                        if (dt.Rows.Count == 0)
                        {
                            appointment.ReminderTime = (ReminderTimeType)reminderTache.SelectedItem;
                            appointment.selectedReminder = reminderTache.SelectedIndex;
                            date = (DateTime)addstarttimeTache.DateTime;
                            appointment.StartTime = ((DateTime)addstartmonthTache.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            date1 = (DateTime)addendtimeTache.DateTime;
                            appointment.EndTime = ((DateTime)addendmonthTache.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            appointment.Subject = subTache.Text;
                            appointment.Notes = notesTache.Text;
                            appointment.selectedActivity = TypeActivite.SelectedIndex;
                            appointment.selectedPriority = priorityTache.SelectedIndex;
                            appointment.selectedEtat = etatTache.SelectedIndex;
                            appointment.AppointmentBackground = colorTache;
                            Taches.UpdateTache(appointment.IdTache, appointment.Subject, appointment.selectedPriority, appointment.StartTime, appointment.selectedEtat, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                            DataTable docs = new DataTable();
                            BL.CLS_Document document = new BL.CLS_Document();
                            docs = document.SelectDocument(SelectedAppointment.IdDocument);
                            try
                            {
                                if (docs.Rows.Count != 0)
                                {
                                    document.UpdateDocument(appointment.IdDocument, this.titleDoc.Text, doc);
                                }
                                else
                                {
                                    document.InsertDocument(this.titleDoc.Text, doc, appointment.IdTache);

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            customeEditorTache.Visibility = Visibility.Collapsed;
                            Schedule.IsHitTestVisible = true;
                        }
                        else
                        {
                            if (dt.Rows.Count == 1)
                            {
                                if ((int)dt.Rows[0]["Id"] == SelectedAppointment.IdTache)
                                {
                                    appointment.ReminderTime = (ReminderTimeType)reminderTache.SelectedItem;
                                    appointment.selectedReminder = reminderTache.SelectedIndex;
                                    date = (DateTime)addstarttimeTache.DateTime;
                                    appointment.StartTime = ((DateTime)addstartmonthTache.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second));
                                    appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                                    date1 = (DateTime)addendtimeTache.DateTime;
                                    appointment.EndTime = ((DateTime)addendmonthTache.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second));
                                    appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                                    appointment.Subject = subTache.Text;
                                    appointment.Notes = notesTache.Text;
                                    appointment.selectedActivity = TypeActivite.SelectedIndex;
                                    appointment.selectedPriority = priorityTache.SelectedIndex;
                                    appointment.selectedEtat = etatTache.SelectedIndex;
                                    appointment.AppointmentBackground = colorTache;
                                    Taches.UpdateTache(appointment.IdTache, appointment.Subject, appointment.selectedPriority, appointment.StartTime, appointment.selectedEtat, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                                    DataTable docs = new DataTable();
                                    BL.CLS_Document document = new BL.CLS_Document();
                                    docs = document.SelectDocument(SelectedAppointment.IdDocument);
                                    try
                                    {
                                        if (docs.Rows.Count != 0)
                                        {
                                            document.UpdateDocument(appointment.IdDocument, this.titleDoc.Text, doc);
                                        }
                                        else
                                        {
                                            document.InsertDocument(this.titleDoc.Text, doc, appointment.IdTache);

                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    customeEditorTache.Visibility = Visibility.Collapsed;
                                    Schedule.IsHitTestVisible = true;
                                }
                                else
                                {
                                    MaterialMessageBox.Show("Une tâche est déjà programmée à cet horaire");
                                }
                            }
                            else
                            {
                                MaterialMessageBox.Show("Une tâche est déjà programmée à cet horaire");
                            }
                        }

                    }
                }
                else
                {
                    MaterialMessageBox.Show("Veuillez revérifier l'horaire");
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }


        private void deleteTache(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = SelectedAppointment;
                if (appointment == null)
                {
                    customeEditorTache.Visibility = Visibility.Collapsed;
                    Schedule.IsHitTestVisible = true;
                }
                else
                {
                    int idTache = (int)appointment.IdTache;
                    Taches.DeleteTache(idTache);
                    if (appointment.IdActivite == 0)
                    {
                        switch (MessageBox.Show("Voulez vous vraiment supprimer La séance du votre emploi du temps ?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                        {
                            case (MessageBoxResult.Yes):
                                cellule.DeleteCellule(appointment.selectedEtat);

                                break;
                        }

                    }
                    Schedule.Appointments.Remove(appointment);
                    customeEditorTache.Visibility = Visibility.Collapsed;
                    Schedule.IsHitTestVisible = true;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void closeTache(object sender, RoutedEventArgs e)
        {
            customeEditorTache.Visibility = Visibility.Collapsed;
            Schedule.IsHitTestVisible = true;
        }

        private void ajouterActivite(object sender, RoutedEventArgs e)
        {

        }

        private void annulerActivite(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region EditorEvent
        private void addAppointmentEvent()
        {
            customeEditorEvent.Visibility = Visibility.Visible;
            Schedule.IsHitTestVisible = false;
            SelectedAppointment = null;
            reminderEvent.ItemsSource = Enum.GetValues(typeof(ReminderTimeType));
            reminderEvent.SelectedIndex = 0;
            addstarttimeEvent.DateTime = eSauve.StartTime;
            addstartmonthEvent.SelectedDate = eSauve.StartTime;
            addendmonthEvent.SelectedDate = eSauve.StartTime.AddHours(1);
            addendtimeEvent.DateTime = eSauve.StartTime.AddHours(1);
            subEvent.Text = "";
            notesEvent.Text = "";
            whereEvent.Text = "";
            titleDoc.Text = "";
        }

        private void editAppointmentEvent()
        {
            try
            {
                customeEditorEvent.Visibility = Visibility.Visible;
                Schedule.IsHitTestVisible = false;
                DataContext = AddDataContext;
                SelectedAppointment = AddDataContext.Appointment as Appointment;
                subEvent.Text = SelectedAppointment.Subject;
                notesEvent.Text = SelectedAppointment.Notes;
                whereEvent.Text = SelectedAppointment.Location;
                reminderEvent.ItemsSource = Enum.GetValues(typeof(ReminderTimeType));
                reminderEvent.SelectedIndex = SelectedAppointment.selectedReminder;
                addstarttimeEvent.DateTime = (AddDataContext.Appointment as Appointment).StartTime;
                addstartmonthEvent.SelectedDate = (AddDataContext.Appointment as Appointment).StartTime;
                addendtimeEvent.DateTime = (AddDataContext.Appointment as Appointment).EndTime;
                addendmonthEvent.SelectedDate = (AddDataContext.Appointment as Appointment).EndTime;
                DataTable docev = new DataTable();
                docev = documentEvent.SelectDocument(SelectedAppointment.IdDocument);
                if (docev.Rows.Count != 0)
                {
                    try
                    {
                        this.titleDocEveHome.Text = (string)docev.Rows[0]["Titre"];
                        doc = (string)docev.Rows[0]["Emplacement"];
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void saveEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment;
                if (Schedule.SelectedAppointment == null)
                {
                    appointment = new Appointment();
                    appointment.TypeAjout = TypeAjout.EVENT;
                }
                else
                {
                    appointment = SelectedAppointment;
                }
                DateTime date = (DateTime)addstarttimeEvent.DateTime;
                DateTime date1 = (DateTime)addendtimeEvent.DateTime;
                if (((DateTime)addstartmonthEvent.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second)) <= ((DateTime)addendmonthEvent.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second)))
                {
                    if (SelectedAppointment == null)
                    {
                        if (Event.SimulEvent(((DateTime)addstartmonthEvent.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second)), ((DateTime)addendmonthEvent.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second)), MainWindow.idUser).Rows.Count == 0)
                        {
                            appointment.ReminderTime = (ReminderTimeType)reminderEvent.SelectedItem;
                            appointment.selectedReminder = reminderEvent.SelectedIndex;
                            appointment.StartTime = ((DateTime)addstartmonthEvent.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            appointment.EndTime = ((DateTime)addendmonthEvent.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            appointment.Subject = subEvent.Text;
                            appointment.Notes = notesEvent.Text;
                            appointment.Location = whereEvent.Text;
                            appointment.AppointmentBackground = colorEvent;
                            appointment.AppointmentImageURI = new BitmapImage(new Uri("pack://application:,,,/AideMemoireFianle1;Component/IconeAppointment/star.png"));
                            Event.InsertEvenement(appointment.Subject, appointment.StartTime, appointment.Location, userId, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                            Events.Clear();
                            Events = Event.SelectEvenement(userId);
                            DataRow dr = Events.Rows[(Events.Rows.Count) - 1];
                            appointment.IdEvenement = (int)dr["Id"];
                            Schedule.Appointments.Add(appointment);
                            try
                            {
                                documentEvent.InsertDocument(this.titleDocEveHome.Text, doc, appointment.IdEvenement);
                            }
                            catch (Exception ex)
                            {

                            }
                            customeEditorEvent.Visibility = Visibility.Collapsed;
                            Schedule.IsHitTestVisible = true;
                        }
                        else
                        {
                            MaterialMessageBox.Show("Un evènement est déjà programmé à cet horaire");
                        }
                    }
                    else
                    {
                        DataTable dt = Event.SimulEvent(((DateTime)addstartmonthEvent.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second)), ((DateTime)addendmonthEvent.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second)), MainWindow.idUser);
                        if (dt.Rows.Count == 0)
                        {
                            appointment.ReminderTime = (ReminderTimeType)reminderEvent.SelectedItem;
                            appointment.selectedReminder = reminderEvent.SelectedIndex;
                            appointment.StartTime = ((DateTime)addstartmonthEvent.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            appointment.EndTime = ((DateTime)addendmonthEvent.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second));
                            appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                            appointment.Subject = subEvent.Text;
                            appointment.Notes = notesEvent.Text;
                            appointment.Location = whereEvent.Text;
                            appointment.AppointmentBackground = colorEvent;
                            Event.UpdateEvenement(appointment.IdEvenement, appointment.Subject, appointment.StartTime, appointment.Location, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                            try
                            {
                                DataTable docev = new DataTable();
                                docev = documentEvent.SelectDocument(SelectedAppointment.IdDocument);
                                if (docev.Rows.Count != 0)
                                {
                                    documentEvent.UpdateDocument(appointment.IdDocument, this.titleDocEveHome.Text, doc);
                                }
                                else
                                {
                                    documentEvent.InsertDocument(this.titleDocEveHome.Text, doc, appointment.IdEvenement);

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            customeEditorEvent.Visibility = Visibility.Collapsed;
                            Schedule.IsHitTestVisible = true;
                        }
                        else
                        {
                            if (dt.Rows.Count == 1)
                            {
                                if ((int)dt.Rows[0]["Id"] == SelectedAppointment.IdEvenement)
                                {
                                    appointment.ReminderTime = (ReminderTimeType)reminderEvent.SelectedItem;
                                    appointment.selectedReminder = reminderEvent.SelectedIndex;
                                    appointment.StartTime = ((DateTime)addstartmonthEvent.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second));
                                    appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                                    appointment.EndTime = ((DateTime)addendmonthEvent.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second));
                                    appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                                    appointment.Subject = subEvent.Text;
                                    appointment.Notes = notesEvent.Text;
                                    appointment.Location = whereEvent.Text;
                                    appointment.AppointmentBackground = colorEvent;
                                    Event.UpdateEvenement(appointment.IdEvenement, appointment.Subject, appointment.StartTime, appointment.Location, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                                    try
                                    {
                                        DataTable docev = new DataTable();
                                        docev = documentEvent.SelectDocument(SelectedAppointment.IdDocument);
                                        if (docev.Rows.Count != 0)
                                        {
                                            documentEvent.UpdateDocument(appointment.IdDocument, this.titleDocEveHome.Text, doc);
                                        }
                                        else
                                        {
                                            documentEvent.InsertDocument(this.titleDocEveHome.Text, doc, appointment.IdEvenement);

                                        }
                                    }
                                    catch (Exception ex)
                                    { }
                                    customeEditorEvent.Visibility = Visibility.Collapsed;
                                    Schedule.IsHitTestVisible = true;
                                }
                                else
                                {
                                    MessageBox.Show("Un évènement est déjà programmé à cet horaire");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Un évènement est déjà programmé à cet horaire");
                            }
                        }
                    }
                }
                else
                {
                    MaterialMessageBox.Show("Veuillez revérifier l'horaire");
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        public static void SetImageForAppointement(Appointment app)
        {
            app.AppointmentImageURI = new BitmapImage(new Uri(""));
        }

        private void deleteEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = SelectedAppointment;
                if (appointment == null)
                {
                    customeEditorEvent.Visibility = Visibility.Collapsed;
                    Schedule.IsHitTestVisible = true;
                }
                else
                {
                    int idEvent = (int)appointment.IdEvenement;
                    Event.DeleteEvenement(idEvent);
                    Schedule.Appointments.Remove(appointment);
                    customeEditorEvent.Visibility = Visibility.Collapsed;
                    Schedule.IsHitTestVisible = true;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void closeEvent(object sender, RoutedEventArgs e)
        {
            customeEditorEvent.Visibility = Visibility.Collapsed;
            Schedule.IsHitTestVisible = true;
        }
        #endregion

        #region Alert

        private void Schedule_ReminderOpening(object sender, ReminderControlOpeningEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.IsHitTestVisible = false;
                player.SoundLocation = @"../../PL/Alarm_Tone_Sound_Clips_From_Orange_Free_Sounds.wav";
                reminder = new Reminder();
                reminder.Closed += reminder_Closed;
                reminder.ReminderAppCollection = e.RemindAppCollection as ScheduleAppointmentCollection;
                reminder.Show();
                player.Play();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une  juj erreur est survenue");
            }
        }

        void reminder_Closed(object sender, EventArgs e)
        {
            this.IsHitTestVisible = true;
            player.Stop();
        }

        #endregion

        #region Interface's events

        private void changeSheduleType(object sender, RoutedEventArgs e)
        {
            string targetView = ((Button)sender).Tag.ToString();
            if (targetView == "Day")
                Schedule.ScheduleType = Syncfusion.UI.Xaml.Schedule.ScheduleType.Day;
            else if (targetView == "Week")
                Schedule.ScheduleType = Syncfusion.UI.Xaml.Schedule.ScheduleType.Week;
            else if (targetView == "Month")
                Schedule.ScheduleType = Syncfusion.UI.Xaml.Schedule.ScheduleType.Month;
        }

        #endregion

        private void activity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addstarttimeEvent_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ajouterDocsEvent(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfileDialog = new OpenFileDialog();
            if (openfileDialog.ShowDialog() == true)
            {
                doc = openfileDialog.FileName;
                this.titleDocEveHome.Text = doc.Split('\\').Last();
            }
        }

        private void OpenFileTache(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(doc));
            }
            catch (Exception ex)
            { }
        }

        private void OpenFileEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(doc));
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void openDialogActivity(object sender, RoutedEventArgs e)
        {
            addActivity.IsOpen = true;
        }

        private void annulerAjouterActivite(object sender, RoutedEventArgs e)
        {
            addActivity.IsOpen = false;
        }

        private void sauveActivity(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.CLS_Activite activites = new BL.CLS_Activite();
                DataTable dt;
                dt = activites.RechActivite(newActivite.Text, TypeActivite.Text, MainWindow.idUser);
                Activite.Items.Clear();
                if (dt.Rows.Count == 0)
                {
                    Activity.InsertActivite(newActivite.Text, TypeActivite.Text, MainWindow.idUser);
                    BL.CLS_Activite activite = new BL.CLS_Activite();
                    dt = activite.SelectActivite(MainWindow.idUser);
                    int i = 0;
                    foreach (DataRow ligne in dt.Rows)
                    {
                        if ((String)ligne["Designation"] != "Planning") Activite.Items.Add((String)ligne["Designation"]);
                        i++;
                    }
                }
                else
                {
                    MaterialMessageBox.Show("L'activité est déjà présente");
                }
                addActivity.IsOpen = false;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }
    }
}




#region Appointment Class

public enum TypeAjout
{
    TASK, EVENT, COURSE
}

public class Appointment : ScheduleAppointment, INotifyPropertyChanged
{

    #region public properties
    private TypeAjout typeAjout;


    public int IdTache { get; set; }
    public int IdCellule { get; set; }
    public int IdActivite { get; set; }
    public int IdEvenement { get; set; }
    public int IdDocument { get; set; }
    public string activite { get; set; }
    public string Typeactivite { get; set; }
    public string AppointmentTime { get; set; }
    public string Professeur { get; set; }


    #endregion

    private void OnPropertyChanged(string propertyName)
    {
        var eventHandler = PropertyChanged;
        if (eventHandler != null)
        {
            eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public AppointmentActivity Appointmentactivity { get; set; }
    public int selectedActivity;
    public enum AppointmentActivity
    {
        Etude,
        Sport,
        Culture
    }

    public AppointmentDays AppointmentDay { get; set; }
    public int selectedDay;
    public enum AppointmentDays
    {
        Samedi,
        Dimanche,
        Lundi,
        Mardi,
        Mercredi,
        Jeudi,
    }

    public AppointmentPriority Appointmentpriority { get; set; }
    public int selectedPriority;
    public enum AppointmentPriority
    {
        Elevée,
        Moyenne,
        Faible
    }

    public AppointmentEtat Appointmentetat { get; set; }
    public int selectedEtat;
    public enum AppointmentEtat
    {
        NonRéalisé,
        EnCours,
        Réalisé
    }

    public AppointmentTypeModule Appointmenttypemodule { get; set; }
    public int selectedTypeModule;
    public enum AppointmentTypeModule
    {
        Cours,
        TD,
        TP
    }

    public string Abbriviation { get; set; }

    public int selectedReminder;

    public event PropertyChangedEventHandler PropertyChanged;


    private ImageSource _imageuri;
    public ImageSource AppointmentImageURI
    {
        get { return _imageuri; }
        set
        {
            _imageuri = value;
            OnPropertyChanged("AppointmentImageURI");
        }
    }


    public TypeAjout TypeAjout { get; set; }






}

#endregion

#region Binding Class

public class BindingClass
{
    public DateTime? CurrentSelectedDate { get; set; }

    public object Appointment { get; set; }
}

#endregion


