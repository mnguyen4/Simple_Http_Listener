using System.ServiceProcess;

namespace Simple_Http_Listener
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            LogUtils.writeLog("Reading config...");
            string hostName = ConfigReader.getStringConfig("hostName");
            LogUtils.writeLog("Hostname: " + hostName);
            string[] hostNames = hostName.Split(';');
            bool useHostsFile = ConfigReader.getBoolConfig("useHostsFile");
            LogUtils.writeLog("Use Hosts File: " + useHostsFile);
            bool useHttps = ConfigReader.getBoolConfig("useHttps");
            LogUtils.writeLog("Use HTTPS: " + useHttps);
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ListenerService(hostNames, useHostsFile, useHttps)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
