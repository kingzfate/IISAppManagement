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
        /// Stop the application and its pool
        /// </summary>
        /// <param name="appName">Application name in IIS</param>
        public void StopApp(string appName)
        {
            if (srvMgr.Sites[appName].State != ObjectState.Stopped &&
                srvMgr.Sites[appName].State != ObjectState.Stopping)
                srvMgr.Sites[appName].Stop();

            if (srvMgr.ApplicationPools[appName].State != ObjectState.Stopped &&
                srvMgr.ApplicationPools[appName].State != ObjectState.Stopping)
                srvMgr.ApplicationPools[appName].Stop();
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