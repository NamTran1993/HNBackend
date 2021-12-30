using HNBackend.Module.TConfig;
using HNBackend.Module.TExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TMySQL
{
    public class TMySQLConnection
    {
        public static TMySQLConnection _TMySQLConnection = null;
        public static OdbcConnection _OdbcConn = null;

        private TMySQLConfig _mySQLConfig = null;
        private static string _pathConfig = string.Empty;

        private string _SECTION = "DatabaseServer";


        private TMySQLConnection()
        {
            TConnectDatabase();
        }

        public static void TInstance(string configDatabase)
        {
            try
            {
                if (!configDatabase.TIsExistFile())
                    throw new Exception("File config database not Exists.");

                _pathConfig = configDatabase;

                if (_TMySQLConnection == null)
                    _TMySQLConnection = new TMySQLConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void TCloseConnection()
        {
            try
            {
                if (_OdbcConn != null && _OdbcConn.State == System.Data.ConnectionState.Open)
                    _OdbcConn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet TExeQuery(string sqlQuery)
        {
            DataSet res = new DataSet();
            try
            {
                if (_OdbcConn != null && _OdbcConn.State == System.Data.ConnectionState.Open)
                {
                    OdbcDataAdapter adpter = new OdbcDataAdapter(sqlQuery, _OdbcConn);
                    adpter.Fill(res);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int TExeNoneQuery(string sqlQuery)
        {
            int res = 0;
            try
            {
                if (_OdbcConn != null && _OdbcConn.State == System.Data.ConnectionState.Open)
                {
                    OdbcCommand odbcCommand = null;
                    odbcCommand = new OdbcCommand(sqlQuery, _OdbcConn);
                    res = odbcCommand.ExecuteNonQuery();
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TConnectDatabase()
        {
            try
            {
                TReadConfig();
                TConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void TReadConfig()
        {
            try
            {
                _mySQLConfig = new TMySQLConfig();

                _mySQLConfig.ServerName = TConfigINI.ReadConfig(_pathConfig, _SECTION, "servername", "");
                _mySQLConfig.Dirver = TConfigINI.ReadConfig(_pathConfig, _SECTION, "driver", "");
                _mySQLConfig.DatabaseName = TConfigINI.ReadConfig(_pathConfig, _SECTION, "databasename", "");
                _mySQLConfig.UserName = TConfigINI.ReadConfig(_pathConfig, _SECTION, "username", "");
                _mySQLConfig.Password = TConfigINI.ReadConfig(_pathConfig, _SECTION, "password", "");
                _mySQLConfig.Port = TConfigINI.ReadConfig(_pathConfig, _SECTION, "port", "").TToInt32();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TConnection()
        {
            try
            {
                if (_mySQLConfig != null)
                {
                    string strConn = string.Format("Driver={0};Server={1};UID={2};PWD={3};DB={4};Port={5}",
                       _mySQLConfig.Dirver, _mySQLConfig.ServerName, _mySQLConfig.UserName, _mySQLConfig.Password, _mySQLConfig.DatabaseName, _mySQLConfig.Port);

                    _OdbcConn = new OdbcConnection(strConn);
                    _OdbcConn.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
