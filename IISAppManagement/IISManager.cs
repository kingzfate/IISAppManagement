using Microsoft.Web.Administration;

namespace IISAppManagement
{
    /// <summary>
    /// Manager for working with iis and server side
    /// </summary>
    public class IISManager
    {
        private readonly ServerManager srvMgr = new ServerManager();

        /// <summary>
        /// Stop all applications and pools
        /// </summary>
        public void StopAllApp()
        {
            foreach (Site site in srvMgr.Sites) StopSite(site.Name);
            foreach (ApplicationPool pool in srvMgr.ApplicationPools) StopPool(pool.Name);
        }

        public void StartAllApp()
        {
            foreach (Site site in srvMgr.Sites) StartSite(site.Name);
            foreach (ApplicationPool pool in srvMgr.ApplicationPools) StartPool(pool.Name);
        }

        /// <summary>
        /// Stop the application and its pool
        /// </summary>
        /// <param name="appName">Application name in IIS</param>
        public void StopApp(string appName)
        {
            StopSite(appName);
            StopPool(appName);
        }

        /// <summary>
        /// Start the application and its pool
        /// </summary>
        /// <param name="appName">Application name in IIS</param>
        public void StartApp(string appName)
        {
            while (srvMgr.Sites[appName].State == ObjectState.Stopping) { }
            srvMgr.Sites[appName].Start();
            while (srvMgr.ApplicationPools[appName].State == ObjectState.Stopping) { }
            srvMgr.ApplicationPools[appName].Start();
        }

        /// <summary>
        /// Start the selected site
        /// </summary>
        /// <param name="siteName">Selected site name</param>
        private void StartSite(string siteName)
        {
            while (srvMgr.Sites[siteName].State == ObjectState.Stopping) { }
            srvMgr.Sites[siteName].Start();
        }

        /// <summary>
        /// Start the selected pool
        /// </summary>
        /// <param name="poolName">Selected pool name</param>
        private void StartPool(string poolName)
        {
            while (srvMgr.ApplicationPools[poolName].State == ObjectState.Stopping) { }
            srvMgr.ApplicationPools[poolName].Start();
        }

        /// <summary>
        /// Stop the selected site
        /// </summary>
        /// <param name="siteName">Selected site name</param>
        private void StopSite(string siteName)
        {
            if (srvMgr.Sites[siteName].State != ObjectState.Stopped &&
                srvMgr.Sites[siteName].State != ObjectState.Stopping)
                srvMgr.Sites[siteName].Stop();
        }

        /// <summary>
        ///  Stop the selected pool
        /// </summary>
        /// <param name="poolName">Selected pool name</param>
        private void StopPool(string poolName)
        {
            if (srvMgr.ApplicationPools[poolName].State != ObjectState.Stopped &&
                srvMgr.ApplicationPools[poolName].State != ObjectState.Stopping)
                srvMgr.ApplicationPools[poolName].Stop();
        }

        /// <summary>
        /// Get a list of all applications
        /// </summary>
        /// <returns>Site collection</returns>
        public SiteCollection GetSites() => srvMgr.Sites;

        /// <summary>
        /// Close the manager and all streams
        /// </summary>
        public void CloseApplication() => srvMgr.Dispose();

        /// <summary>
        /// Get application status
        /// </summary>
        /// <returns>ObjectState</returns>
        public ObjectState GetAppState(string appName) => srvMgr.Sites[appName].State;

        /// <summary>
        /// Get pool status
        /// </summary>
        /// <returns>ObjectState</returns>
        public ObjectState GetPoolState(string appName) => srvMgr.ApplicationPools[appName].State;
    }
}