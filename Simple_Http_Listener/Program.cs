using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ListenerService(hostNames, useHostsFile)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
