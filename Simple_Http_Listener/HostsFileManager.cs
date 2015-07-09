using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Http_Listener
{
    class HostsFileManager
    {
        private const string HOSTS_FILE_PATH = "C:\\Windows\\System32\\drivers\\etc\\hosts";

        [DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
        private static extern UInt32 DnsFlushResolverCache();

        private string hostsFile;
        private string hostsBackup;

        public HostsFileManager()
        {
            hostsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hosts\\hosts");
            hostsBackup = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hosts\\hosts.back");
        }

        public void applyHostsFile()
        {
            if (!copyHostsFile(HOSTS_FILE_PATH, hostsBackup) || !copyHostsFile(hostsFile, HOSTS_FILE_PATH))
            {
                return;
            }
            flushDnsCache();
        }

        public void restoreHostsFile()
        {
            if (!copyHostsFile(hostsBackup, HOSTS_FILE_PATH))
            {
                return;
            }
            flushDnsCache();
        }

        private bool copyHostsFile(string source, string destination)
        {
            bool succeed;
            try
            {
                File.Copy(source, destination, true);
                succeed = true;
            }
            catch (Exception e)
            {
                LogUtils.writeLog(e.StackTrace);
                succeed = false;
            }
            return succeed;
        }

        private void flushDnsCache()
        {
            UInt32 result = DnsFlushResolverCache();
        }
    }
}
