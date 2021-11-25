using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TSQLite
{
    public abstract class TSQLiteConnection
    {
        private SQLiteConnection _connection = null;
        protected SQLiteConnection Con { get => _connection; }

        private int _version = 3;
        public int Version { get => _version; set => _version = value; }

        private string _fullPathFileSQLite = string.Empty;

        protected void CreateSQLiteFile(string fullPathFileSQLite)
        {
            try
            {
                if (string.IsNullOrEmpty(fullPathFileSQLite))
                    throw new Exception("Full Path File SQLite IsNullOrEmpty.");

                _fullPathFileSQLite = fullPathFileSQLite;

                SQLiteConnection.CreateFile(fullPathFileSQLite);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void OpenConnection(string fullPathFileSQLite)
        {
            try
            {
                if (string.IsNullOrEmpty(fullPathFileSQLite))
                    throw new Exception("ConnectionString IsNullOrEmpty.");

                if (!File.Exists(fullPathFileSQLite))
                    throw new Exception(string.Format("{0} not Exists.", fullPathFileSQLite));

                _fullPathFileSQLite = fullPathFileSQLite;

                string strConn = string.Format("Data Source={0};Version={1};", fullPathFileSQLite, _version);
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    _connection = new SQLiteConnection() { ConnectionString = strConn };
                    _connection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void CloseConnection()
        {
            try
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ExcuteQuery(string sqlQuery, bool bCloseConnection = false)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception("SQL Query IsNullOrEmpty.");

                OpenConnection(_fullPathFileSQLite);

                SQLiteCommand sqlCommand = new SQLiteCommand(sqlQuery, _connection);
                sqlCommand.ExecuteNonQuery();

                if (bCloseConnection)
                    CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected DataSet Select(string sqlQuery, bool bCloseConnection = false)
        {
            DataSet res = null;
            try
            {
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception("SQL Query IsNullOrEmpty.");

                res = new DataSet();

                OpenConnection(_fullPathFileSQLite);

                SQLiteDataAdapter da = new SQLiteDataAdapter(sqlQuery, _connection);
                da.Fill(res);

                if (bCloseConnection)
                    CloseConnection();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
