using DisPatch.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DisPatch.Common.Helpers;

namespace DisPatch.DB.Implements
{
    internal class MSRepository: IBaseRepository
    {
        /// <summary>
        /// 数据库连接字串
        /// </summary>
        private string connStr = "";

        public MSRepository(string _connStr)
        {
            connStr = _connStr;
        }

        public IDbConnection GetConnection(string _connStr)
        {
            if (!string.IsNullOrEmpty(_connStr))
                connStr = _connStr;

            if (string.IsNullOrEmpty(connStr))
                connStr = AppsettingHelper.ReadAppSettings("DB", "Connect");

            return new SqlConnection(connStr);
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
                    return conn.QueryFirstOrDefault<string>(sql, commandTimeout: commandTimeout, commandType: CommandType.Text);
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.QueryFirstOrDefault<string>(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
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
