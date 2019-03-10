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
using System.Data;
using System.Collections.ObjectModel;
using BespokeFusion;
using System.Media;

namespace Projet.PL
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : Page
    {

        #region Members

        internal Appointment SelectedAppointment;
        internal BindingClass AddDataContext = null;
        Reminder reminder;
        SoundPlayer player = new SoundPlayer();
        public AppointmentEditorOpeningEventArgs eSauve;
        ScheduleAppointmentCollection AppointmentCollection = new ScheduleAppointmentCollection();
        ObservableCollection<SolidColorBrush> brush = new ObservableCollection<SolidColorBrush>();
        BL.CLS_Evenement Event = new BL.CLS_Evenement();
        BL.CLS_DocumentEvent documentEvent = new BL.CLS_DocumentEvent();
        int userId = MainWindow.idUser;

        DataTable evenement = new DataTable();
        string doc;

        #endregion

        public Events()
        {
            try
            {
                InitializeComponent();
                Contacts contact = new Contacts();
                customeEditorEvent.Visibility = Visibility.Collapsed;
                Schedule.FirstDayOfWeek = Setting.firstDay;
                Schedule.ReminderOpening += Schedule_ReminderOpening;
                Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xA2, 0xC1, 0x39)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xD8, 0x00, 0x73)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x1B, 0xA1, 0xE2)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xF0, 0x96, 0x09)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x33, 0x99, 0x33)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x00, 0xAB, 0xA9)));
                brush.Add(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));
                evenement = Event.SelectEvenement(userId);
                if (this.evenement != null)
                {
                    foreach (DataRow ligne in this.evenement.Rows)
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
                    }
                }
                Schedule.Appointments = AppointmentCollection;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
        }

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
                    editAppointmentEvent();
                }
                else
                {
                    addAppointmentEvent();
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
                this.titleDocEve.Text = doc.Split('\\').Last();
            }
        }

        #endregion

        #region EditorEvent
        private void addAppointmentEvent()
        {
            customeEditorEvent.Visibility = Visibility.Visible;
            Schedule.IsHitTestVisible = false;
            SelectedAppointment = null;
            addstarttimeEvent.DateTime = eSauve.StartTime;
            addstartmonthEvent.SelectedDate = eSauve.StartTime;
            reminderEvent.ItemsSource = Enum.GetValues(typeof(ReminderTimeType));
            reminderEvent.SelectedIndex = 0;
            addendmonthEvent.SelectedDate = eSauve.StartTime.AddHours(1);
            addendtimeEvent.DateTime = eSauve.StartTime.AddHours(1);
            subEvent.Text = "";
            notesEvent.Text = "";
            whereEvent.Text = "";
            titleDocEve.Text = "";
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
                addstarttimeEvent.DateTime = (AddDataContext.Appointment as Appointment).StartTime;
                addstartmonthEvent.SelectedDate = (AddDataContext.Appointment as Appointment).StartTime;
                addendtimeEvent.DateTime = (AddDataContext.Appointment as Appointment).EndTime;
                addendmonthEvent.SelectedDate = (AddDataContext.Appointment as Appointment).EndTime;
                reminderEvent.ItemsSource = Enum.GetValues(typeof(ReminderTimeType));
                reminderEvent.SelectedIndex = SelectedAppointment.selectedReminder;
                DataTable docev = new DataTable();
                docev = documentEvent.SelectDocument(SelectedAppointment.IdDocument);
                if (docev.Rows.Count != 0)
                {
                    try
                    {
                        this.titleDocEve.Text = (string)docev.Rows[0]["Titre"];
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
                        //appointment.AppointmentBackground = colorEvent;
                        Event.InsertEvenement(appointment.Subject, appointment.StartTime, appointment.Location, userId, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                        evenement.Clear();
                        evenement = Event.SelectEvenement(userId);
                        DataRow dr = evenement.Rows[(evenement.Rows.Count) - 1];
                        appointment.IdEvenement = (int)dr["Id"];
                        Schedule.Appointments.Add(appointment);
                        try
                        {
                            documentEvent.InsertDocument(this.titleDocEve.Text, doc, appointment.IdEvenement);
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
                        //appointment.AppointmentBackground = colorEvent;
                        Event.UpdateEvenement(appointment.IdEvenement, appointment.Subject, appointment.StartTime, appointment.Location, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                        try
                        {
                            DataTable docev = new DataTable();
                            docev = documentEvent.SelectDocument(SelectedAppointment.IdDocument);
                            if (docev.Rows.Count != 0)
                            {
                                documentEvent.UpdateDocument(appointment.IdDocument, this.titleDocEve.Text, doc);
                            }
                            else
                            {
                                documentEvent.InsertDocument(this.titleDocEve.Text, doc, appointment.IdEvenement);

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
                                //appointment.AppointmentBackground = colorEvent;
                                Event.UpdateEvenement(appointment.IdEvenement, appointment.Subject, appointment.StartTime, appointment.Location, appointment.EndTime, appointment.Notes, appointment.selectedReminder);
                                try
                                {
                                    DataTable docev = new DataTable();
                                    docev = documentEvent.SelectDocument(SelectedAppointment.IdDocument);
                                    if (docev.Rows.Count != 0)
                                    {
                                        documentEvent.UpdateDocument(appointment.IdDocument, this.titleDocEve.Text, doc);
                                    }
                                    else
                                    {
                                        documentEvent.InsertDocument(this.titleDocEve.Text, doc, appointment.IdEvenement);

                                    }
                                }
                                catch (Exception ex)
                                { }
                                customeEditorEvent.Visibility = Visibility.Collapsed;
                                Schedule.IsHitTestVisible = true;
                            }
                            else
                            {
                                MaterialMessageBox.Show("Un évènement est déjà programmé à cet horaire");
                            }
                        }
                        else
                        {
                            MaterialMessageBox.Show("Un évènement est déjà programmé à cet horaire");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }

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
        private void goToEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                addAppointmentEvent();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Une erreur est survenue");
            }
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

        private void OpenEventFile(object sender, RoutedEventArgs e)
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

        private void ajouterDocsEvent(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfileDialog = new OpenFileDialog();
            if (openfileDialog.ShowDialog() == true)
            {
                doc = openfileDialog.FileName;
                this.titleDocEve.Text = doc.Split('\\').Last();
            }
        }

        private void OpenFileEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(doc));
            }
            catch (Exception ex)
            { }
        }
    }
}


