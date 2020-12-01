using Microsoft.Web.Administration;
using System;
using System.Threading;
using System.Windows.Forms;

namespace IISAppManagement
{
    /// <summary>
    /// Main class without form
    /// </summary>
    public partial class Main : Form
    {
        private readonly ServerManager mgr = new ServerManager();

        public Main()
        {
            InitializeComponent();
            SettingNotifyIcon();
        }

        /// <summary>
        /// Setting up the context menu in the application tray
        /// </summary>
        private void SettingNotifyIcon()
        {
            notifyIconMain.ContextMenuStrip = new ContextMenuStrip();

            foreach (Site site in mgr.Sites)
            {
                notifyIconMain.ContextMenuStrip.Items.Add(site.Name, null, Reboot_Click);
            }
            notifyIconMain.ContextMenuStrip.Items.Add("Exit", null, AppExit_Click);
        }

        /// <summary>
        /// Reboot instance. Pool and application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reboot_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            mgr.Sites[item.Text].Stop();
            mgr.ApplicationPools[item.Text].Stop();
            Thread.Sleep(2000);
            mgr.Sites[item.Text].Start();
            mgr.ApplicationPools[item.Text].Start();
        }

        /// <summary>
        /// Unload managers and close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppExit_Click(object sender, EventArgs e) 
        {
            mgr.Dispose();
            System.Windows.Forms.Application.Exit(); 
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000;
                return cp;
            }
        }
    }
}