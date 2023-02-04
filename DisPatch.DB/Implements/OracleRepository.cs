using Dapper;
using DisPatch.Common.Helpers;
using DisPatch.DB.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.DB.Implements
{
    internal class OracleRepository : IBaseRepository
    {
        /// <summary>
        /// 数据库连接字串
        /// </summary>
        private string connStr = "";

        public OracleRepository(string _connStr)
        {
            connStr = _connStr;
        }

        [Obsolete]
        public IDbConnection GetConnection(string _connStr)
        {
            if (!string.IsNullOrEmpty(_connStr))
                connStr = _connStr;

            if (string.IsNullOrEmpty(connStr))
                //connStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                connStr = AppsettingHelper.ReadAppSettings("DB", "Connect");

            return new OracleConnection(connStr);
        }

        public int ExecuteSqlInt(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(connStr))
                {
                    conn.Open();
                    return conn.Execute(sql, commandTimeout: commandTimeout, commandType: CommandType.Text);
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.Execute(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
            }
        }

        public string Query1RecordString(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(connStr))
                {
                    conn.Open();
                    return (string)conn.QueryFirstOrDefault(sql, commandTimeout: commandTimeout, commandType: CommandType.Text);
                }
            }
            else
            {
                var conn = transaction.Connection;
                return (string)conn.QueryFirstOrDefault(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
            }
        }

        public T Query1Record<T>(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(connStr))
                {
                    conn.Open();
                    return conn.QueryFirstOrDefault<T>(sql, commandTimeout: commandTimeout, commandType: CommandType.Text);
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.QueryFirstOrDefault<T>(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
            }
        }

        public IEnumerable<T> QueryRecords<T>(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(connStr))
                {
                    conn.Open();
                    return conn.Query<T>(sql, commandTimeout: commandTimeout, commandType: CommandType.Text);
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.Query<T>(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
            }
        }

        public DataTable QueryTable(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(connStr))
                {
                    conn.Open();
                    var reader = conn.ExecuteReader(sql, commandTimeout: commandTimeout, commandType: CommandType.Text);
                    DataTable result = new DataTable();
                    result.Load(reader);
                    return result;
                }
            }
            else
            {
                var conn = transaction.Connection;
                var reader = conn.ExecuteReader(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
                DataTable result = new DataTable();
                result.Load(reader);
                return result;
            }
        }
    }
}
