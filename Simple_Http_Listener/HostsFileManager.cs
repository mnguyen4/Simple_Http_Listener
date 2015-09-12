using System;
using System.IO;
using System.Runtime.InteropServices;

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
            if (!File.Exists(hostsBackup))
            {
                copyHostsFile(HOSTS_FILE_PATH, hostsBackup);
            }
            if (copyHostsFile(hostsFile, HOSTS_FILE_PATH))
            {
                flushDnsCache();
            }
        }

        public void restoreHostsFile()
        {
            if (copyHostsFile(hostsBackup, HOSTS_FILE_PATH))
            {
                flushDnsCache();
            }
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
