using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Http_Listener
{
    class LogUtils
    {
        public static string LOG_LOCATION = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "log.txt");

        public static void writeLog(string stringToLog)
        {
            // append string to log file
            File.AppendAllText(LOG_LOCATION, stringToLog + "\n");
        }
    }
}
