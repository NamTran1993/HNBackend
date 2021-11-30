using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Global
{
    public class TGlobal
    {
        public static string _PATH_LOG = string.Empty;
        public static string _PATH_SQLITE = string.Empty;
        public static string _FILE_NAME_SQLITE = string.Empty;

        private static object _object = new object();

        #region WRITE LOG
        public static void InitFolderLog(string baseDirectory)
        {
            try
            {
                if (string.IsNullOrEmpty(baseDirectory))
                    throw new Exception("baseDirectory IsNullOrEmpty.");

                _PATH_LOG = string.Format("{0}\\{1}", baseDirectory, "LogFiles");

                if (!Directory.Exists(_PATH_LOG))
                {
                    try
                    {
                        Directory.CreateDirectory(_PATH_LOG);
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Log(string fileName, string content, TYPE_LOGGER typeLogger = TYPE_LOGGER.NORMAL)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                    return;

                string folderLogFile = Path.Combine(_PATH_LOG, DateTime.UtcNow.ToString("yyyy-MM-dd"));
                string full_path = string.Empty;

                lock (_object)
                {
                    switch (typeLogger)
                    {
                        case TYPE_LOGGER.DEBUG:
                            string folder_debug = string.Format("{0}\\{1}", folderLogFile, "DEBUG");
                            if (!Directory.Exists(folder_debug))
                                Directory.CreateDirectory(folder_debug);
                            full_path = string.Format("{0}\\{1}", folder_debug, fileName);
                            break;

                        case TYPE_LOGGER.WARNING:
                            string folder_warning = string.Format("{0}\\{1}", folderLogFile, "WARNING");
                            if (!Directory.Exists(folder_warning))
                                Directory.CreateDirectory(folder_warning);
                            full_path = string.Format("{0}\\{1}", folder_warning, fileName);
                            break;

                        case TYPE_LOGGER.ERROR:
                            {
                                string folder_error = string.Format("{0}\\{1}", folderLogFile, "ERROR");
                                if (!Directory.Exists(folder_error))
                                    Directory.CreateDirectory(folder_error);
                                full_path = string.Format("{0}\\{1}", folder_error, fileName);
                                break;
                            }
                        default:
                            {
                                string folder = folderLogFile;
                                if (!Directory.Exists(folder))
                                    Directory.CreateDirectory(folder);
                                full_path = string.Format("{0}\\{1}", folder, fileName);
                                break;
                            }
                    }

                    // -- 
                    try
                    {
                        using (FileStream file_stream = File.Open(full_path, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            DateTime dtNow = DateTime.Now;
                            /* Example Log
                             * 12/5/2019 9:42:00 AM - Microsoft VSIX Installer
                               12/5/2019 9:42:00 AM - -------------------------------------------
                             **/
                            string date_time = dtNow.ToString("yyyy-MM-dd hh:mm:ss tt");
                            string line = "----------[END]----------\r\n";
                            string fullContent = string.Format("{0}{1} - {2} {3} {4}", "\n", date_time, content, "\r\n", line);

                            long offset = file_stream.Seek(0, SeekOrigin.End);
                            ASCIIEncoding encoding = new ASCIIEncoding();

                            byte[] arrLogs = encoding.GetBytes(fullContent);
                            file_stream.Write(arrLogs, 0, arrLogs.Length);

                            file_stream.Close();
                            file_stream.Dispose();
                        }
                    }
                    catch
                    { }
                }
            }
            catch (Exception)
            { }
        }

        #endregion

        #region GUID
        public static string CreateGUID(TGUID type = TGUID.DEFAULT)
        {
            string guid_result = string.Empty;
            try
            {
                string guid = Guid.NewGuid().ToString();
                string[] arrGuid = guid.Split('-');
                if (arrGuid != null && arrGuid.Length > 0)
                {
                    switch (type)
                    {
                        case TGUID.DATE:
                            string strDate = DateTime.Now.ToString("yyyy-MM-dd");
                            guid_result = string.Format("{0}-{1}-{2}", strDate, arrGuid[0], arrGuid[1]);
                            break;
                        case TGUID.TIME:
                            string strTime = DateTime.Now.ToString("HH-mm-ss");
                            guid_result = string.Format("{0}-{1}-{2}", strTime, arrGuid[0], arrGuid[1]);
                            break;
                        case TGUID.DEFAULT_2:
                            guid_result = string.Format("{0}-{1}", arrGuid[0], arrGuid[2]);
                            break;
                        case TGUID.DEFAULT_3:
                            guid_result = string.Format("{0}-{1}", arrGuid[0], arrGuid[3]);
                            break;
                        case TGUID.DEFAULT_4:
                            guid_result = string.Format("{0}-{1}", arrGuid[0], arrGuid[4]);
                            break;
                        default:
                            guid_result = string.Format("{0}-{1}", arrGuid[0], arrGuid[1]);
                            break;
                    }
                }

                return guid_result;
            }
            catch (Exception ex)
            { }
            return string.Empty;
        }
        #endregion

        #region SQLITE
        public static void InitPathSQLite(string baseDirectory)
        {
            try
            {
                if (string.IsNullOrEmpty(baseDirectory))
                    throw new Exception("baseDirectory IsNullOrEmpty.");

                _PATH_SQLITE = string.Format("{0}\\{1}", baseDirectory, "SQLite");
                if (!Directory.Exists(_PATH_SQLITE))
                    Directory.CreateDirectory(_PATH_SQLITE);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region TIME
        public static long ConvertDateTimeToInt64(DateTime dtValue)
        {
            long lvalue = dtValue.Year * 10000000000 + dtValue.Month * 100000000
                    + dtValue.Day * 1000000 + dtValue.Hour * 10000 + dtValue.Minute * 100 + dtValue.Second;
            return lvalue;
        }
        public static DateTime ConvertInt64ToDateTime(long lvalue)
        {
            try
            {
                if (lvalue <= 0)
                    return DateTime.MinValue;

                int year = (int)(lvalue / 10000000000);
                int month = (int)((lvalue % 10000000000) / 100000000);
                int day = (int)((lvalue % 100000000) / 1000000);
                int hour = (int)((lvalue % 1000000) / 10000);
                int minute = (int)((lvalue % 10000) / 100);
                int second = (int)(lvalue % 100);
                DateTime dtValue = new DateTime(year, month, day, hour, minute, second);
                return dtValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static long ParseDateTimeToTick(string str)
        {
            if (str.Length > 0)
            {
                try
                {
                    str = str.Replace('-', '/');
                    DateTime date = DateTime.ParseExact(str, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return date.Ticks;
                }
                catch (Exception ex)
                { }
            }
            return 0;
        }
        #endregion

        #region DIFF
        public static string ConvertByteArrayToString(byte[] input)
        {
            string res = string.Empty;
            try
            {
                var hex = new StringBuilder(input.Length * 2);
                foreach (byte b in input)
                    hex.AppendFormat("{0:x2}", b);
                return hex.ToString();
            }
            catch (Exception ex)
            {
                res = string.Empty;
            }
            return res;
        }

        public static byte[] ConvertStringToByteArray(string hex)
        {
            byte[] res = null;
            try
            {
                if (hex == null || hex == string.Empty)
                    return null;
                return Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();
            }
            catch (Exception ex)
            {
                res = null;
            }
            return res;
        }

        public static void ZipFolder(string pathFolder, string outputFile, int compressionLevel = 9)
        {
            try
            {
                string[] fileNames = Directory.GetFiles(pathFolder);
                if (fileNames != null && fileNames.Length > 0)
                {
                    using (ZipOutputStream outputStream = new ZipOutputStream(File.Create(outputFile)))
                    {
                        outputStream.SetLevel(compressionLevel);
                        byte[] buffer = new byte[4096];

                        foreach (string file in fileNames)
                        {
                            ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                            entry.DateTime = DateTime.Now;
                            outputStream.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(file))
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                    outputStream.Write(buffer, 0, sourceBytes);
                                } while (sourceBytes > 0);

                                fs.Close();
                                fs.Dispose();
                            }
                        }

                        outputStream.Finish();
                        outputStream.Close();
                        try
                        {
                            if (Directory.Exists(pathFolder))
                                Directory.Delete(pathFolder, false);
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
