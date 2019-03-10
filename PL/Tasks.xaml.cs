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
using System.Collections.ObjectModel;
using System.Data;
using System.Media;
using BespokeFusion;

namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : Page
    {

        #region Members

        internal Appointment SelectedAppointment;
        internal BindingClass AddDataContext = null;
        public AppointmentEditorOpeningEventArgs eSauve;
        ScheduleAppointmentCollection AppointmentCollection = new ScheduleAppointmentCollection();
        ObservableCollection<SolidColorBrush> brush = new ObservableCollection<SolidColorBrush>();
        Reminder reminder;
        SoundPlayer player = new SoundPlayer();
        BL.CLS_Tache Taches = new BL.CLS_Tache();
        BL.CLS_Cellule cellule = new BL.CLS_Cellule();
        BL.CLS_Activite Activity = new BL.CLS_Activite();
        int userId = MainWindow.idUser;
        int IdentifiantEmploiDuTemps;
        DataTable Activities = new DataTable();
        DataTable taches = new DataTable();
        SolidColorBrush colorTache = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c3e50"));
        BL.CLS_Document document = new BL.CLS_Document();
        string doc;
        #endregion

        #region Constructor

        public Tasks()
        {
            try
            {
                InitializeComponent();
                customeEditorTache.DataContext = this;
                customeEditorTache.Visibility = Visibility.Collapsed;
                Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
                Schedule.ReminderOpening += Schedule_ReminderOpening;
                Activities = Activity.SelectActivite(userId);
                foreach (DataRow ligneact in this.Activities.Rows)
                {
                    taches = Taches.SelectTache((int)ligneact["Id"]);
                    if (this.taches != null)
                    {
                        foreach (DataRow ligne in this.taches.Rows)
                        {
                            int iddoc;
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
                                iter = 19;
                                if (IdentifiantEmploiDuTemps == 0)
                                {
                                    IdentifiantEmploiDuTemps = (int)ligne["ActiviteId"];
                                }
                            }

                            while (cpt < iter)
                            {
                                AppointmentCollection.Add(new Appointment()
                                {

                                    StartTime = date1,
                                    EndTime = date2,
                                    AppointmentBackground = colorTache,
                                    Subject = (string)ligne["Designation"],
                                    selectedEtat = (int)ligne["Etat"],
                                    TypeAjout = TypeAjout.TASK,
                                    selectedPriority = (int)ligne["Priorite"],
                                    IdActivite = (int)ligne["ActiviteId"],
                                    IdTache = (int)ligne["Id"],
                                    IdCellule = (int)ligne["Etat"],
                                    Notes = (string)ligne["Commentaire"],
                                    selectedActivity = ((int)ligne["ActiviteId"] - 1),
                                    IdDocument = iddoc,
                                    selectedReminder = (int)ligne["Alerte"]
                                });
                                date1 = date1.AddDays(semaine);
                                date2 = date2.AddDays(semaine);
                                cpt++;


                            }
                        }
                    }

                }
                Schedule.Appointments = AppointmentCollection;
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
                AddDataContext = new BindingClass() { CurrentSelectedDate = e.StartTime, Appointment = e.Appointment };
                if (e.Appointment != null)
                {
                    editAppointmentTache();

                }
                else
                {
                    addAppointmentTache();
                }
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
                    int idTache = appointment.IdTache;
                    Taches.DeleteTache(idTache);
                    if (appointment.IdActivite == 0)
                    {
                        switch (MessageBox.Show("Voulez vous  supprimer La séance du votre emploi du temps ?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                        {
                            case (MessageBoxResult.Yes):
                                cellule.DeleteCellule(appointment.IdCellule);

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
                MaterialMessageBox.Show("Une erreur est survenue");
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
        private void goToTache(object sender, RoutedEventArgs e)
        {
            addAppointmentTache();
        }
        #endregion

        private void activity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OpenFileTache(object sender, RoutedEventArgs e)
        {
            try
            {

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(doc));
            }
            catch (Exception ex)
            {

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