﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Http_Listener
{
    class ConfigReader
    {
        public static string getStringConfig(string key) {
            string result;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                // get the value of the setting or empty string if null
                result = appSettings[key] ?? "";
            }
            catch (ConfigurationErrorsException e)
            {
                LogUtils.writeLog(e.StackTrace);
                result = "";
            }
            return result;
        }

        public static bool getBoolConfig(string key)
        {
            bool result = getStringConfig(key).Equals("true");
            return result;
        }
    }
}
