using System;
using System.IO;

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
