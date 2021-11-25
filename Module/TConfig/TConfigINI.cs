using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TConfig
{
    public class TConfigINI
    {
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public static string ReadConfig(string filePathIni, string section, string key, string defaultValue)
        {
            string res = defaultValue;
            try
            {
                if (File.Exists(filePathIni))
                {
                    var retVal = new StringBuilder(1024);
                    GetPrivateProfileString(section, key, defaultValue, retVal, 1024, filePathIni);
                    return retVal.ToString();
                }
            }
            catch (Exception ex)
            {
                res = defaultValue;
            }
            return res;
        }

        public static bool WriteConfig(string filePathIni, string section, string key, string value)
        {
            bool res = false;
            try
            {
                res = WritePrivateProfileString(section, key, value, filePathIni);
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
    }
}
