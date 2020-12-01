using IISAppManagement.Properties;
using Microsoft.Web.Administration;
using System;
using System.Windows.Forms;

namespace IISAppManagement
{
    /// <summary>
    /// The main application for information processing
    /// </summary>
    public class Application : ApplicationContext
    {
        private readonly NotifyIcon trayIcon = new NotifyIcon();
        private readonly IISManager iisMgr = new IISManager();

        public Application()
        {
            SettingsNotify();
        }

        /// <summary>
        /// Tray app setup
        /// </summary>
        private void SettingsNotify()
        {
            trayIcon.Icon = Resources.main;
            trayIcon.Visible = true;
            trayIcon.ContextMenuStrip = new ContextMenuStrip();

            foreach (Site site in iisMgr.GetSites())
            {
                trayIcon.ContextMenuStrip.Items.Add(site.Name, null, Reboot_Click);
            }
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, AppExit_Click);
        }

        /// <summary>
        /// Reboot instance. Pool and application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reboot_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            iisMgr.StopApp(item.Text);
            iisMgr.StartApp(item.Text);
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