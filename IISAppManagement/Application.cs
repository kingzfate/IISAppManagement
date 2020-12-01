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
        private const string runApp = "Run";
        private const string stopApp = "Stop";
        private const string rebootApp = "Reboot";

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

            ToolStripMenuItem Run = new ToolStripMenuItem { Name = runApp, Text = "Запустить" };
            ToolStripMenuItem Stop = new ToolStripMenuItem { Name = stopApp, Text = "Остановить" };
            ToolStripMenuItem Reboot = new ToolStripMenuItem { Name = rebootApp, Text = "Перезагрузка" };
            Run.Click += InstanceContext_Click;
            Stop.Click += InstanceContext_Click;
            Reboot.Click += InstanceContext_Click;

            foreach (Site site in iisMgr.GetSites())
            {
                ToolStripMenuItem newElement = new ToolStripMenuItem { Name = site.Name, Text = site.Name };
                newElement.DropDownItems.AddRange(new ToolStripItem[] { Run, Stop, Reboot });
                trayIcon.ContextMenuStrip.Items.Add(newElement);
            }
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, AppExit_Click);
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