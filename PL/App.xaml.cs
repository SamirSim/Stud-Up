using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace Projet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private System.Windows.Forms.NotifyIcon icone;
        private static bool isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new PL.MainWindow();
            //  MainWindow.Closing += MainWindow_Closing;
            // PL.MainWindow.window.Closing += Window_Closing;
            this.icone = new System.Windows.Forms.NotifyIcon();
            this.icone.DoubleClick += (s, args) => ShowMainWindow();
            this.icone.Icon = AideMemoireFianle1.Properties.Resources.MyIcon;
            this.icone.Visible = true;
            CreateContextMenu();
        }


        private void CreateContextMenu()
        {
            this.icone.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.icone.ContextMenuStrip.Items.Add("Ouvrir StudUP").Click += (s, e) => ShowMainWindow();
            this.icone.ContextMenuStrip.Items.Add("Quitter").Click += (s, e) => ExitApplication();

        }

        private void ExitApplication()
        {
            isExit = true;
            if (PL.MainWindow.window != null)
            {
                PL.MainWindow.window.Close();
            }
            this.icone.Dispose();
            this.icone = null;
            Application.Current.Shutdown();
        }

        private void ShowMainWindow()
        {
            try
            {
                if (PL.MainWindow.window.IsVisible)
                {
                    if (PL.MainWindow.window.WindowState == WindowState.Minimized)
                    {
                        PL.MainWindow.window.WindowState = WindowState.Normal;
                    }
                    PL.MainWindow.window.Activate();
                }
                else
                {
                    PL.MainWindow.window.Show();
                }
            }
            catch
            {
                PL.MainWindow window = new PL.MainWindow();
                window.Show();
            }

        }

        public static void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!isExit)
            {
                e.Cancel = true;
                PL.MainWindow.window.Hide(); // A hidden window can be shown again, a closed one not
            }
        }

    }
}
