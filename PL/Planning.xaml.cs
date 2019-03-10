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
using Microsoft.Win32;
using System.Data;
using System.Drawing;
using BespokeFusion;

using System.Collections.ObjectModel;


namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for Planning.xaml
    /// </summary>
    public partial class Planning : Page
    {
        #region Members

        internal Appointment SelectedAppointment;
        internal BindingClass AddDataContext = null;
        public AppointmentEditorOpeningEventArgs eSauve;
        ScheduleAppointmentCollection AppointmentCollection = new ScheduleAppointmentCollection();
        ObservableCollection<SolidColorBrush> brush = new ObservableCollection<SolidColorBrush>();
        DataTable emploisutemps = new DataTable();
        DataTable Taches = new DataTable();
        DataTable Activities = new DataTable();
        BL.CLS_Cellule cellule = new BL.CLS_Cellule();
        BL.CLS_Tache tache = new BL.CLS_Tache();
        BL.CLS_Activite activite = new BL.CLS_Activite();
        public int IdentifiantEmploiDuTemps;
        int userId = MainWindow.idUser;
        static int nb = 0;

        #endregion

        #region Constructor

        public Planning()
        {
            try
            {
                InitializeComponent();
                Schedule1.FirstDayOfWeek = Setting.firstDay;
                if (Setting.firstHour == "8:00") Schedule1.WorkStartHour = 8;
                if (Setting.firstHour == "9:00") Schedule1.WorkStartHour = 9;
                if (Setting.firstHour == "10:00") Schedule1.WorkStartHour = 10;
                if (Setting.lastHour == "20:00") Schedule1.WorkEndHour = 20;
                if (Setting.lastHour == "21:00") Schedule1.WorkEndHour = 21;
                if (Setting.lastHour == "22:00") Schedule1.WorkEndHour = 22;
                customeEditorCourse.Visibility = Visibility.Collapsed;
                Schedule1.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xA2, 0xC1, 0x39)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xD8, 0x00, 0x73)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x1B, 0xA1, 0xE2)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xF0, 0x96, 0x09)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x33, 0x99, 0x33)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x00, 0xAB, 0xA9)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));
                if (IdentifiantEmploiDuTemps == 0)
                {
                    bool Found = false;
                    Activities = activite.SelectActivite(userId);
                    int cpt1 = 0;
                    DataRow ligneact;
                    while ((cpt1 < Activities.Rows.Count) && (Found == false))
                    {
                        ligneact = Activities.Rows[cpt1];
                        if ((string)ligneact["Designation"] == "Planning")
                        {
                            IdentifiantEmploiDuTemps = (int)ligneact["Id"];
                            Found = true;
                        }
                        else
                        {
                            cpt1++;
                        }
                    }
                }
                this.emploisutemps = cellule.SelectCellule(userId);
                if (this.emploisutemps != null)
                {
                    this.emploisutemps.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                    int i = 1;
                    DateTime d = DateTime.Now;
                    DateTime dateSamedi;
                    int add = 0;
                    switch (d.DayOfWeek)
                    {
                        case (DayOfWeek.Saturday):
                            add = 0;
                            break;

                        case (DayOfWeek.Sunday):
                            add = -1;
                            break;
                        case (DayOfWeek.Monday):
                            add = -2;
                            break;
                        case (DayOfWeek.Tuesday):
                            add = -3;
                            break;
                        case (DayOfWeek.Wednesday):
                            add = -4;
                            break;
                        case (DayOfWeek.Thursday):
                            add = -5;
                            break;
                        case (DayOfWeek.Friday):
                            add = -6;
                            break;
                    }
                    dateSamedi = d.AddDays(add);
                    foreach (DataRowView ligne in this.emploisutemps.DefaultView)
                    {
                        d = dateSamedi.AddDays((int)ligne["Jour"]);
                        DateTime date1 = new DateTime(d.Year, d.Month, d.Day, int.Parse(((string)ligne["HeureDebut"]).Substring(0, 2)), int.Parse(((string)ligne["HeureDebut"]).Substring(3, 2)), 0);
                        DateTime date2 = new DateTime(d.Year, d.Month, d.Day, int.Parse(((string)ligne["HeureFin"]).Substring(0, 2)), int.Parse(((string)ligne["HeureFin"]).Substring(3, 2)), 0);
                        AppointmentCollection.Add(new Appointment()
                        {
                            Abbriviation = (string)ligne["Abrv"],
                            StartTime = date1,
                            EndTime = date2,
                            selectedDay = (int)ligne["Jour"],
                            selectedTypeModule = (int)ligne["Type"],
                            AppointmentBackground = brush[i],
                            IdCellule = (int)ligne["Id"],
                            Subject = (string)ligne["Designation"],
                            Location = (string)ligne["Salle"],
                            Professeur = (string)ligne["Enseignant"],//enseignant
                        });
                        i++;
                        if (i > 4) { i = 1; }
                    }
                    Schedule1.Appointments = AppointmentCollection;
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
            Appointment app;
            app = (Appointment)(e.Appointment);
            eSauve = e;
            e.Cancel = true;
            Schedule1.IsHitTestVisible = false;
            AddDataContext = new BindingClass() { CurrentSelectedDate = e.StartTime, Appointment = e.Appointment };
            if (e.Appointment != null)
            {
                editAppointmentCourse();
            }
            else
            {
                addAppointmentCourse();
            }
        }

        #endregion

        private void ajouterDocs(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) { }
        }

        #region EditorCourse

        private void addAppointmentCourse()
        {
            try
            {
                nb = 0;
                customeEditorCourse.Visibility = Visibility.Visible;
                Schedule1.IsHitTestVisible = false;
                addstarttimeTache.IsEnabled = true;
                addendtimeTache.IsEnabled = true;
                SelectedAppointment = null;
                day.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentDays));
                day.SelectedIndex = 0;
                typeModule.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentTypeModule));
                typeModule.SelectedIndex = 0;
                addstarttimeTache.DateTime = eSauve.StartTime;
                addstartmonthTache.SelectedDate = eSauve.StartTime;
                addendmonthTache.SelectedDate = eSauve.StartTime.AddHours(0);
                addendtimeTache.DateTime = eSauve.StartTime.AddHours(3);
                if (eSauve.StartTime.DayOfWeek == DayOfWeek.Saturday) day.Text = "Samedi";
                if (eSauve.StartTime.DayOfWeek == DayOfWeek.Sunday) day.Text = "Dimanche";
                if (eSauve.StartTime.DayOfWeek == DayOfWeek.Monday) day.Text = "Lundi";
                if (eSauve.StartTime.DayOfWeek == DayOfWeek.Tuesday) day.Text = "Mardi";
                if (eSauve.StartTime.DayOfWeek == DayOfWeek.Wednesday) day.Text = "Mercredi";
                if (eSauve.StartTime.DayOfWeek == DayOfWeek.Thursday) day.Text = "Jeudi";
                if (eSauve.StartTime.DayOfWeek == DayOfWeek.Friday) day.Text = "Vendredi";
                day.IsEnabled = true;
                subModule.Text = "";
                abrvModule.Text = "";
                notesModule.Text = "";
                salle.Text = "";
                prof.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        private void editAppointmentCourse()
        {
            try
            {
                customeEditorCourse.Visibility = Visibility.Visible;
                Schedule1.IsHitTestVisible = false;
                DataContext = AddDataContext;
                SelectedAppointment = AddDataContext.Appointment as Appointment;
                day.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentDays));
                day.SelectedIndex = SelectedAppointment.selectedDay;
                day.IsEnabled = false;
                typeModule.ItemsSource = Enum.GetValues(typeof(Appointment.AppointmentTypeModule));
                typeModule.SelectedIndex = SelectedAppointment.selectedTypeModule;
                subModule.Text = SelectedAppointment.Subject;
                salle.Text = SelectedAppointment.Location;
                prof.Text = SelectedAppointment.Professeur;
                abrvModule.Text = SelectedAppointment.Abbriviation;
                addstarttimeTache.DateTime = (AddDataContext.Appointment as Appointment).StartTime;
                addstartmonthTache.SelectedDate = (AddDataContext.Appointment as Appointment).StartTime;
                addendtimeTache.DateTime = (AddDataContext.Appointment as Appointment).EndTime;
                addendmonthTache.SelectedDate = (AddDataContext.Appointment as Appointment).EndTime;
                addstarttimeTache.DateTime = (AddDataContext.Appointment as Appointment).StartTime;
                addstarttimeTache.IsEnabled = false;
                addendtimeTache.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void saveCourse(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment;
                if (Schedule1.SelectedAppointment == null)
                {
                    appointment = new Appointment();
                    appointment.TypeAjout = (TypeAjout.COURSE);
                }
                else
                {
                    appointment = SelectedAppointment;
                }
                DateTime date = (DateTime)addstarttimeTache.DateTime;
                DateTime date1 = (DateTime)addendtimeTache.DateTime;
                appointment.EndTime = ((DateTime)addendmonthTache.SelectedDate).Date.Add(new TimeSpan(date1.Hour, date1.Minute, date1.Second));
                appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                if (nb == 0)
                {
                    appointment.Subject = typeModule.Text + " " + subModule.Text;
                    nb++;
                }
                else appointment.Subject = subModule.Text;
                appointment.Notes = notesModule.Text;
                appointment.Abbriviation = abrvModule.Text;
                appointment.Location = salle.Text;
                appointment.selectedTypeModule = typeModule.SelectedIndex;
                appointment.selectedDay = day.SelectedIndex;
                appointment.Professeur = prof.Text;
                appointment.StartTime = ((DateTime)addstartmonthTache.SelectedDate).Date.Add(new TimeSpan(date.Hour, date.Minute, date.Second));
                appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                if (Schedule1.SelectedAppointment == null)
                {
                    DateTime d = DateTime.Now;
                    DateTime dateSamedi;
                    int add = 0;
                    switch (d.DayOfWeek)
                    {
                        case (DayOfWeek.Saturday):
                            add = 0;
                            break;
                        case (DayOfWeek.Sunday):
                            add = -1;
                            break;
                        case (DayOfWeek.Monday):
                            add = -2;
                            break;
                        case (DayOfWeek.Tuesday):
                            add = -3;
                            break;
                        case (DayOfWeek.Wednesday):
                            add = -4;
                            break;
                        case (DayOfWeek.Thursday):
                            add = -5;
                            break;
                        case (DayOfWeek.Friday):
                            add = -6;
                            break;
                    }
                    dateSamedi = d.AddDays(add);
                    DateTime datejour = dateSamedi.AddDays(appointment.selectedDay);
                    date = (DateTime)addstarttimeTache.DateTime;
                    DateTime start = new DateTime(datejour.Year, datejour.Month, datejour.Day, date.Hour, date.Minute, date.Second);
                    appointment.StartTime = start;
                    date1 = (DateTime)addendtimeTache.DateTime;
                    DateTime end = new DateTime(datejour.Year, datejour.Month, datejour.Day, date1.Hour, date1.Minute, date1.Second);
                    appointment.EndTime = end;
                    appointment.AppointmentTime = appointment.StartTime.ToString("HH:mm tt");
                    BL.CLS_Cellule celulle = new BL.CLS_Cellule();
                    DataTable cours;
                    cours = cellule.SimulCours(start.ToString("HH:mm"), end.ToString("HH:mm"), appointment.selectedDay, MainWindow.idUser);
                    if (cours.Rows.Count == 0)
                    {
                        cellule.InsertCellule(appointment.Abbriviation, appointment.Subject, start.ToString("HH:mm"), end.ToString("HH:mm"), appointment.Location, appointment.selectedDay, appointment.selectedTypeModule, appointment.Professeur, userId);
                        emploisutemps.Clear();
                        emploisutemps = cellule.SelectCellule(userId);
                        int cpt = 0;
                        DataRow dr;
                        int maxId = -1;
                        int id;
                        while ((cpt < emploisutemps.Rows.Count))
                        {
                            dr = emploisutemps.Rows[cpt];
                            id = (int)dr["Id"];
                            if (id > maxId)
                            {
                                maxId = id;
                            };
                            cpt++;
                        }
                        appointment.IdCellule = maxId;
                        tache.InsertTache(appointment.Subject, appointment.selectedTypeModule, start, appointment.IdCellule, IdentifiantEmploiDuTemps, end, "", 0, MainWindow.idUser);
                        Schedule1.Appointments.Add(appointment);
                    }
                    else
                    {
                        MaterialMessageBox.Show("Un cours est déjà programmé à cet horaire ");
                    }
                }

                else
                {
                    appointment = SelectedAppointment;
                    cellule.UpdateCellule(appointment.IdCellule, appointment.Abbriviation, appointment.Subject, date.ToString("HH:mm"), date1.ToString("HH:mm"), appointment.Location, appointment.selectedDay, appointment.selectedTypeModule, appointment.Professeur);
                    Taches = tache.SelectTache(IdentifiantEmploiDuTemps);
                    int j = 0;
                    DataRow ligneTache;
                    bool updated = false;
                    try
                    {
                        while ((j < Taches.Rows.Count) && (updated == false))
                        {
                            ligneTache = Taches.Rows[j];
                            if (((int)ligneTache["Etat"] == appointment.IdCellule) && ((int)ligneTache["ActiviteId"] == IdentifiantEmploiDuTemps))
                            {
                                tache.UpdateTache((int)ligneTache["Id"], appointment.Subject, appointment.selectedTypeModule, appointment.StartTime, appointment.IdCellule, appointment.EndTime, "Laseance :" + appointment.Abbriviation + "a la salle " + appointment.Location, 0);
                                updated = true;
                            }
                            else
                            {
                                j++;
                            }

                        }
                        customeEditorCourse.Visibility = Visibility.Collapsed;
                    }
                    catch (NullReferenceException ex1)
                    {
                        MessageBox.Show(ex1.Message);

                    }
                    catch (Exception ex1)
                    {
                        MessageBox.Show(ex1.Message);
                    }
                }
                customeEditorCourse.Visibility = Visibility.Collapsed;
                Schedule1.IsHitTestVisible = true;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

        private void deleteCourse(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointment appointment = SelectedAppointment;
                if (appointment == null)
                {
                    customeEditorCourse.Visibility = Visibility.Collapsed;
                    Schedule1.IsHitTestVisible = true;
                }
                else
                {
                    int idcellule = (int)appointment.IdCellule;
                    cellule.DeleteCellule(idcellule);
                    Schedule1.Appointments.Remove(appointment);
                    MaterialMessageBox.Show("Le cours est supprimé avec succès !");
                    Taches = tache.SelectTache(IdentifiantEmploiDuTemps);
                    int j = 0;
                    DataRow ligneTache;
                    bool deleted = false;
                    try
                    {
                        while ((j < Taches.Rows.Count) && (deleted == false))
                        {
                            ligneTache = Taches.Rows[j];
                            if (((int)ligneTache["Etat"] == appointment.IdCellule) && ((int)ligneTache["ActiviteId"] == IdentifiantEmploiDuTemps))
                            {
                                tache.DeleteTache((int)ligneTache["Id"]);
                                deleted = true;
                            }
                            else
                            {
                                j++;
                            }

                        }

                    }
                    catch (NullReferenceException ex1)
                    {
                        MaterialMessageBox.Show(ex1.Message);

                    }
                    catch (Exception ex1)
                    {
                        MaterialMessageBox.Show(ex1.Message);
                    }
                    customeEditorCourse.Visibility = Visibility.Collapsed;
                    Schedule1.IsHitTestVisible = true;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }

        }

        private void closeCourse(object sender, RoutedEventArgs e)
        {
            customeEditorCourse.Visibility = Visibility.Collapsed;
            Schedule1.IsHitTestVisible = true;
        }
        #endregion
    }

}
