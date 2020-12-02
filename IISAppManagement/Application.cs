using IISAppManagement.Properties;
using Microsoft.Web.Administration;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IISAppManagement
{
    /// <summary>
    /// The main application for information processing
    /// </summary>
    public class Application : ApplicationContext
    {
        private readonly IISManager iisMgr = new IISManager();
        private readonly NotifyIcon trayIcon = new NotifyIcon();
        private const string runApp = "Run";
        private const string stopApp = "Stop";
        private const string rebootApp = "Reboot";
        ToolStripMenuItem Run, Stop, Reboot;

        public Application()
        {
            SettingsNotify();
        }

        /// <summary>
        /// Tray app setup
        /// </summary>
        private void SettingsNotify()
        {
            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            trayIcon.Icon = Resources.main;
            trayIcon.Visible = true;
            trayIcon.Click += Notify_Click;
            trayIcon.Text = "IISAppManagement";
            Run = new ToolStripMenuItem { Name = runApp, Text = runApp };
            Stop = new ToolStripMenuItem { Name = stopApp, Text = stopApp };
            Reboot = new ToolStripMenuItem { Name = rebootApp, Text = rebootApp };
            Run.Click += InstanceContext_Click;
            Stop.Click += InstanceContext_Click;
            Reboot.Click += InstanceContext_Click;
            AddMenuItem();
        }

        /// <summary>
        /// Handling a click on a tray icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notify_Click(object sender, EventArgs e)
        {
            trayIcon.ContextMenuStrip.Items.Clear();
            AddMenuItem();
        }

        /// <summary>
        /// Add and recalculate context menu items
        /// </summary>
        public void AddMenuItem()
        {
            foreach (Site site in iisMgr.GetSites())
            {
                ToolStripMenuItem newElement = new ToolStripMenuItem { Name = site.Name, Text = site.Name, Image = GetImageStatusConnection(site.Name) };                
                newElement.DropDownItems.AddRange(new ToolStripItem[] { Run, Stop, Reboot });
                trayIcon.ContextMenuStrip.Items.Add(newElement);
            }
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator { });
            trayIcon.ContextMenuStrip.Items.Add("Enable all", null, EnableAllApp_Click);
            trayIcon.ContextMenuStrip.Items.Add("Disable all", null, DisableAllApp_Click);
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, AppExit_Click);
        }

        /// <summary>
        /// Enable all sites and application pools
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableAllApp_Click(object sender, EventArgs e) => iisMgr.StartAllApp();

        /// <summary>
        /// Disable all sites and application pools
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisableAllApp_Click(object sender, EventArgs e) => iisMgr.StopAllApp();

        /// <summary>
        /// Get the status of connection and application operation as an image
        /// </summary>
        /// <returns></returns>
        private Bitmap GetImageStatusConnection(string appName)
        {
            ObjectState appState = iisMgr.GetAppState(appName);
            ObjectState poolState = iisMgr.GetPoolState(appName);

            if (appState == ObjectState.Stopped || poolState == ObjectState.Stopped) return Resources.stopped;
            else if (appState == ObjectState.Stopping || poolState == ObjectState.Stopping) return Resources.stopping;
            else if (appState == ObjectState.Started && poolState == ObjectState.Started) return Resources.started;
            else if (appState == ObjectState.Started && poolState == ObjectState.Starting) return Resources.starting;
            else if (appState == ObjectState.Starting && poolState == ObjectState.Started) return Resources.starting;
            else if (appState == ObjectState.Starting && poolState == ObjectState.Starting) return Resources.starting;
            else return Resources.stopped;
        }

        /// <summary>
        /// Perform tasks depending on the selected context
        /// Start application, 
        /// Stop application, 
        /// Reload application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstanceContext_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            switch (item.Name)
            {
                case runApp:
                    iisMgr.StartApp(item.OwnerItem.Text);
                    break;
                case stopApp:
                    iisMgr.StopApp(item.OwnerItem.Text);
                    break;
                case rebootApp:
                    iisMgr.StopApp(item.OwnerItem.Text);
                    iisMgr.StartApp(item.OwnerItem.Text);
                    break;
            };
        }

        /// <summary>
        /// Unload managers and close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppExit_Click(object sender, EventArgs e)
        {
            iisMgr.CloseApplication();
            System.Windows.Forms.Application.Exit();
        }
    }
}